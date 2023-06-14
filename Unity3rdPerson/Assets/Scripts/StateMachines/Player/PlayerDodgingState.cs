using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");
    private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");
    private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");

    private Vector3 dodgingDirectionInput;//direction player is moving before dodge, ie. direction to dodge in
    private float remainingDodgeTime;//time left in dodge movement
    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) : base(stateMachine)//constructor with passed Vector3
    {
        this.dodgingDirectionInput = dodgingDirectionInput;//sets the dodging direction to the vector 3 that gets passed on call from targetingState
    }

    public override void Enter()
    {
        remainingDodgeTime = stateMachine.DodgeDuration;//initialises the value of remaining dodge time to passed dodge duration value
        stateMachine.Animator.SetFloat(DodgeForwardHash, dodgingDirectionInput.y);//directional inpouts were normalized already, will work for blend tree.
        stateMachine.Animator.SetFloat(DodgeRightHash, dodgingDirectionInput.x);
        stateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, 0.1f);//change to the dodge blend tree
        stateMachine.Health.SetInvulnerable(true);//Makes player invulnerable
    }
    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();//storage for movement vector

        movement += stateMachine.transform.right * dodgingDirectionInput.x * stateMachine.DodgeLength / stateMachine.DodgeDuration;
        movement += stateMachine.transform.forward * dodgingDirectionInput.y * stateMachine.DodgeLength / stateMachine.DodgeDuration;
        //calculates movement based off of previous movement inputs, distance assigned to dodge, and the time a dodge should last
        Move(movement, deltaTime);//move with new dodge velocity
        FaceTarget();//face the players target
        remainingDodgeTime -= deltaTime;//subtract the time remaining in the dodge
        if(remainingDodgeTime <= 0f)//if the time remaing in the dodge is over, go back to targeting
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }

    }
    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);//end of invulnerability
    }


}
