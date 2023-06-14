using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{

    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed"); //Faster to store numbers than strings. assigned on launch, read only.
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");

    private const float AnimatorDampTime = 0.1f;
    private bool shouldFade;//a variable to store if the animator shoulkd fade between animations or not
    public PlayerFreeLookState(PlayerStateMachine stateMachine,bool shouldFade = true) : base(stateMachine)
        //should fade is an optional peramiter. optional peramiters need to be stored after required peramiters.
    {
            this.shouldFade = shouldFade;//sets the passed should fade peramiter to the local should fade variable
    }

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;//subscribes to the target method
        stateMachine.InputReader.JumpEvent += OnJump;//subscribes to the jump method
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0f);//sets the speedhash to 0 to prevent any unintended animation crossover
        if(shouldFade)//bool for if the animator should fade or not
        {
            stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, 0.1f); //set the player animation with fade
        }
        else
        {
            stateMachine.Animator.Play(FreeLookBlendTreeHash);//play the animation with no fade
        }
        
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.IsAttacking) //able to go to attacking from free look
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
        }

        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);

    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;//unsubscribes to the target method
        stateMachine.InputReader.JumpEvent -= OnJump;//unsubscribes to the jump method
    }

    private void OnTarget()
    {
        if(!stateMachine.Targeter.SelectTarget()) { return; }
        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));//switch to targeting state
    }
    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));//switch to jumping state
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y + right * stateMachine.InputReader.MovementValue.x;
    }

    private void FaceMovementDirection(Vector3 movement,float deltaTime)
    {
        //LERP Linear Interpolate, quaternion a (start) , wuaternion b (what youre rotating to), float t (how far between the values we should go)
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDampening);
    }

}
