using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPullUpState : PlayerBaseState
{
    private readonly int PullUpHash = Animator.StringToHash("PullUp");//store PullUp animation as a hash for faster refrence
    private readonly Vector3 Offset = new Vector3(0f, 2.325f, 0.65f);
    public PlayerPullUpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(PullUpHash, 0.1f);//Play the PullUp animation
    }
    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator, "Climbing") < 1f) { return; }
        //checks to see if the climbing tagged animations are done, since there should only be 1 there shouldnt be issues
        stateMachine.Controller.enabled = false;//turn off the player controller so we can use translate method
        stateMachine.transform.Translate(Offset, Space.Self);//moves the player to the proper location after the Pull up animation,
                                                             //space.self because the position is relative to the player 
        stateMachine.Controller.enabled = true;//turn on the player controller after translation has completed
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine,false));//change to free look state with no animator fading
        
        
    }
    public override void Exit()
    {
        stateMachine.Controller.Move(Vector3.zero);//set move to 0 to not jitter around when climbing
        stateMachine.ForceReciever.Reset();//resets gravity fall force to 0.
    }

}
