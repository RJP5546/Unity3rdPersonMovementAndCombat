using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        stateMachine.Ragdoll.ToggleRagdoll(true);//calls toggle ragdoll to set it true
        stateMachine.Weapon.gameObject.SetActive(false);//turns off the weapon game object, preventing furtur collisions
    }
    public override void Tick(float deltaTime){}
    public override void Exit(){}


}
