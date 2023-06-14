using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private bool alreadyAppliedForce;

    private Attack attack;
    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.Weapon.SetAttack(attack.Damage, attack.KnockBack); //call set attack passing attack damage value from editor value in playerStateMachine
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }
    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        FaceTarget();
        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");//need to pass this state machines animator up to the class "State" and its animation tag
        if (normalizedTime < 1f) //check if the animation is finished or not
        {
            if(normalizedTime >= attack.ForceTime)//try force application
            {
                TryApplyForce();
            }
            if (stateMachine.InputReader.IsAttacking)//try attacking
            {
                TryComboAttack(normalizedTime);
            }
        }
        else //returns to previous states when attack is done
        {
            if(stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }
    }

    public override void Exit()
    {

    }

    private void TryComboAttack(float normalizedTime)
    {
        if(attack.ComboStateIndex == -1) { return; }
        if(normalizedTime < attack.ComboAttackTime) { return; }
        stateMachine.SwitchState
        (
            new PlayerAttackingState(stateMachine, attack.ComboStateIndex)
        );
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) { return; }
        stateMachine.ForceReciever.AddForce(stateMachine.transform.forward * attack.Force);
        alreadyAppliedForce = true;
    }




}
