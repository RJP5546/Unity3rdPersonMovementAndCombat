using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    private readonly int BlockHash = Animator.StringToHash("Block");//store block animation as a hash for faster refrence
    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine){}//constructor

    public override void Enter()
    {
        stateMachine.Health.SetInvulnerable(true);//makes player invunerable
        stateMachine.Animator.CrossFadeInFixedTime(BlockHash, 0.1f);
    }
    public override void Tick(float deltaTime)
    {
        Move(deltaTime);//still want forces to apply to us
        if(!stateMachine.InputReader.IsBlocking)//if no longer blocking return to targeting state
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
        if(stateMachine.Targeter.CurrentTarget == null)//if no active target or target has died, return to free look
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);//makes player take damage
    }


}
