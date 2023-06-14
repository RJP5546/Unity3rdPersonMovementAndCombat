using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Ragdoll.ToggleRagdoll(true);//calls toggle ragdoll to set it true
        stateMachine.Weapon.gameObject.SetActive(false);//turns off the weapon game object, preventing furtur collisions
        GameObject.Destroy(stateMachine.Target);//destroys target component, preventing player from targeting them
    }
    public override void Tick(float deltaTime) { }
    public override void Exit() { }


}
