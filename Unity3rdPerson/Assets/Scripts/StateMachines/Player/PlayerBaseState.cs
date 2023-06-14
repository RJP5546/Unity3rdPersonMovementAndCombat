using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State //abstract class passes down the constructor to anything that inherits from this
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    protected void Move(float deltaTime)//moving the player with no input(attacking or knockback)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)//moving the player via inputs
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReciever.Movement)*deltaTime);
    }

    protected void FaceTarget()
    {
        if(stateMachine.Targeter.CurrentTarget == null) { return; }

        Vector3 lookPos = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos); //converts vector to quaternion
    }

    protected void ReturnToLocomotion()//checks what state to return to after being knocked back
    {
        if(stateMachine.Targeter.CurrentTarget != null)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));//if player has target return to targeting
        }
        else
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));//if no target, return to free look
        }
    }
}
