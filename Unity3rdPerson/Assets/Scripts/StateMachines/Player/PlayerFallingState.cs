using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private readonly int FallHash = Animator.StringToHash("Fall");//store Fall animation as a hash for faster refrence
    private Vector3 momentum;//storing the velocity of movement before jump to maintain direction of travel while jumping
    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0f;

        stateMachine.Animator.CrossFadeInFixedTime(FallHash, 0.1f);

        stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }
    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);//updates movement every frame
        if(stateMachine.Controller.isGrounded) //checks to see if player is on the ground
        {
            ReturnToLocomotion();//returns to proper last state via method in PlayerBaseState
        }
        FaceTarget();
    }
    public override void Exit()
    {
        stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
    }

    private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        stateMachine.SwitchState(new PlayerHangingState(stateMachine, ledgeForward, closestPoint));
        //switches to the hanging state, gives the ledge forward and closest point to player.
    }
}
