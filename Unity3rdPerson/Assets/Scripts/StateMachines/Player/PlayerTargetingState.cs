using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForwardSpeed");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRightSpeed");

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;//subscribes to the target method
        stateMachine.InputReader.DodgeEvent += OnDodge;//subscribes to the dodge method
        stateMachine.InputReader.JumpEvent += OnJump;//subscribes to the dodge method
        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, 0.1f); //set the player animation
    }
    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.IsAttacking) //if the attack button has been pressed, go to attack state
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
        }
        if (stateMachine.InputReader.IsBlocking) //if the block button has been pressed, go to block state
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
        }

        if (stateMachine.Targeter.CurrentTarget == null) //returns to free look if target is gone
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement(deltaTime);
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();

    }
    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;//unsubscribes to the Target method
        stateMachine.InputReader.DodgeEvent -= OnDodge;//unsubscribes to the dodge method
        stateMachine.InputReader.JumpEvent -= OnJump;//unsubscribes to the jump method
    }

    private void OnTarget()//cancel targeting and return to freelook using the same targeting input
    {
        stateMachine.Targeter.Cancel();//calls cancel and sets current target to null
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));//switch to free look
    }
    private void OnDodge()
    {
        if(stateMachine.InputReader.MovementValue == Vector2.zero) { return; }//cannot dodge without directional input
        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, stateMachine.InputReader.MovementValue));//switches to the player dodging state, passes input direction
    }
    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));//switch to jumping state
    }

    private Vector3 CalculateMovement(float deltaTime)
    {
        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x; //Left and right
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y; //Forward and back

        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        if (stateMachine.InputReader.MovementValue.y == 0)
        {
            stateMachine.Animator.SetFloat(TargetingForwardHash, 0, 0.1f, deltaTime);
        }
        else
        {
            float value = stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f; //checks for true and false, uses 1st num if true, 2nd if false
            stateMachine.Animator.SetFloat(TargetingForwardHash, value, 0.1f, deltaTime); //sets the hash value to the result form the line above
        }

        if (stateMachine.InputReader.MovementValue.x == 0)
        {
            stateMachine.Animator.SetFloat(TargetingRightHash, 0, 0.1f, deltaTime);
        }
        else
        {
            float value = stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f; //checks for true and false, uses 1st num if true, 2nd if false
            stateMachine.Animator.SetFloat(TargetingRightHash, value, 0.1f, deltaTime); //sets the hash value to the result form the line above
        }

    }

}
