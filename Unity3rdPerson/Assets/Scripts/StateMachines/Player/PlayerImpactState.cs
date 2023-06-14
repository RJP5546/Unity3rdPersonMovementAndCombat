using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine){}

    private readonly int ImpactHash = Animator.StringToHash("Impact");

    private float duration = 1f;//stun duration after impact
    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, 0.1f);//sets the animation to impact with a 0.1f time fade
    }
    public override void Tick(float deltaTime)//makes sure no movement besides force and gravity are allowed in this state
    {
        Move(deltaTime);

        duration -= deltaTime;

        if(duration <= 0 )//if stun duration hits 0
        {
            ReturnToLocomotion();
        }
    }

    public override void Exit()
    {

    }


}
