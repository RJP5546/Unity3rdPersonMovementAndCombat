using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    private readonly int hangingHash = Animator.StringToHash("Hanging");
    private Vector3 closestPoint;//local storage of the players closest point to the ledge
    private Vector3 ledgeForward;//local storage of the ledge forward orientatioin
    public PlayerHangingState(PlayerStateMachine stateMachine, Vector3 ledgeForward, Vector3 closestPoint) : base(stateMachine)
    {
        this.ledgeForward = ledgeForward;//local storage of the passed players closest point to the ledge
        this.closestPoint = closestPoint;//local storage of the passed ledge forward orientatioin
    }

    public override void Enter()
    {
        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);
        //make the player face the direction of the ledge. it also wants the world up, so we pass vector3.up

        stateMachine.Controller.enabled = false;//Turns off the character controller so we can move the player via transform
        stateMachine.transform.position = closestPoint - (stateMachine.LedgeDetector.transform.position - stateMachine.transform.position);
        //set the player transform to the closest point - the distance between the players hands, and the player. this will make the hands go to the closest point
        stateMachine.Controller.enabled = true;//Turns on the character controller so we can move the player via Controller

        stateMachine.Animator.CrossFadeInFixedTime(hangingHash, 0.1f);//switch to the hanging animation
    }
    public override void Tick(float deltaTime)
    {
        if(stateMachine.InputReader.MovementValue.y < 0)//if the backwards movement input is pressed
        {
            stateMachine.ForceReciever.Reset();
            stateMachine.Controller.Move(Vector3.zero);
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));//drop and switch to falling
        }
        else if(stateMachine.InputReader.MovementValue.y > 0f)//if the forwards movement input is pressed
        {
            stateMachine.SwitchState(new PlayerPullUpState(stateMachine));//switch to pull up state
        }
    }
    public override void Exit()
    {

    }


}
