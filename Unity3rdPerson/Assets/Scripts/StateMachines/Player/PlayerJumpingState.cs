using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private readonly int JumpHash = Animator.StringToHash("Jump");//store Jump animation as a hash for faster refrence
    private Vector3 momentum;//storing the velocity of movement before jump to maintain direction of travel while jumping
    public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        stateMachine.ForceReciever.Jump(stateMachine.JumpForce);

        momentum = stateMachine.Controller.velocity;//stores the current direction of travel.
        momentum.y = 0;//the y is overwritten by the jump.
        stateMachine.Animator.CrossFadeInFixedTime(JumpHash, 0.1f);

        stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;//subscribes from the ledge detction method
    }
    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);//updates movement every frame
        if (-stateMachine.Controller.velocity.y <= 0)
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
        }
        FaceTarget();//will face target if already facing target. ignores if in freelook
    }
    public override void Exit()
    {
        stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;//unsubscribes from the ledge detction method
    }

    private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
        //method gets the forward orientation of the ledge as well as the players closest point to the ledge
    {
        stateMachine.SwitchState(new PlayerHangingState(stateMachine, ledgeForward, closestPoint));
        //when a ledge is detected, switch to hanging state, pass the orientation of the ledge and the players closest point
    }
}
