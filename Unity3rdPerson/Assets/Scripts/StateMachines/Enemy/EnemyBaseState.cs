using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State //abstract class passes down the constructor to anything that inherits from this
{
    protected EnemyStateMachine stateMachine; //takes in the enemy base state

    public EnemyBaseState(EnemyStateMachine stateMachine) //constructor for enemy state machine
    {
        this.stateMachine = stateMachine;
    }
    protected void Move(float deltaTime)//moving the enemy with no input(attacking or knockback)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)//moving the enemy
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReciever.Movement) * deltaTime);
    }
    protected void FacePlayer()
    {
        if (stateMachine.Player == null) { return; }

        Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos); //converts vector to quaternion
    }
    protected bool IsInChaseRange()//protected to allow acess from all children of this class only
    {
        if (stateMachine.Player.IsDead) {return false; }
        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        //uses squares to avoid square roots, faster and easier on machine computing ie. 2*2 is easier than sqrt(4)
        return playerDistanceSqr <= stateMachine.PlayerChasingRange * stateMachine.PlayerChasingRange;
    }
}
