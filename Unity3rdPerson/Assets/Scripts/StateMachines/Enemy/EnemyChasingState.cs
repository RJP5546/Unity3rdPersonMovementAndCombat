using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion"); //enemy animation blend tree to a hash
    private readonly int SpeedHash = Animator.StringToHash("Speed");//enemy animation speed value
    private const float AnimatorDampTime = 0.1f;
    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, 0.1f); //set the enemy animation to the locomotion tree
    }
    public override void Tick(float deltaTime)
    {
        
        if (!IsInChaseRange())  //if in chase range, go to chase state
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
        else if (IsInAttackRange()) //if in attack range, go to attack state
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            return;
        }

        MoveToPlayer(deltaTime);
        FacePlayer();

        stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);//moves to 0 over the course of damp time var
    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();//resets path
        stateMachine.Agent.velocity = Vector3.zero;//sets velocity to 0
    }

    private void MoveToPlayer(float deltaTime)//try to move to player
    {
        if(stateMachine.Agent.isOnNavMesh)//checks to make sure navMesh is enabled before movement
        {
            stateMachine.Agent.destination = stateMachine.Player.transform.position;//sets the destination as the players location

            Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);//moves with normalised velocity with speed
        }
       
        stateMachine.Agent.velocity = stateMachine.Controller.velocity;//makes sure nav mesh agent and char controller are in sync
    }

    private bool IsInAttackRange()//checks to see if player is in attack range
    {
        if (stateMachine.Player.IsDead) { return false; }
        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude; //checks player distance against attack range.
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}
