using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Attack");
    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        FacePlayer();
        stateMachine.Weapon.SetAttack(stateMachine.AttackDamage, stateMachine.Knockback);
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, 0.1f);
    }
    public override void Tick(float deltaTime)
    {
        
        if (GetNormalizedTime(stateMachine.Animator, "Attack") >= 1)//need to pass this state machines animator up to the class "State"
                                                          //and greater than 1 means the animation has finished
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }
        
    }
    public override void Exit()
    {
        
    }


}
