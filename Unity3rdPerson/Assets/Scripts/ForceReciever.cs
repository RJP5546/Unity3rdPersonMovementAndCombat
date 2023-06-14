using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReciever : MonoBehaviour
{

    [SerializeField] private CharacterController controller;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float drag = 0.3f; //Smooth time on the forces dampening

    private float verticalVelocity;
    private Vector3 impact; //impact vector
    private Vector3 dampingVelocity;//used as temp storage in the damping

    public Vector3 Movement => impact + Vector3.up * verticalVelocity; // => means return whats on the right
                                                                       // adds movement based on gravity and forces (knockback and others)
    

    private void Update()
    {
        if (verticalVelocity < 0f && controller.isGrounded) //is on the ground
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else //gravity
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);//reduces vecrot 3 towards target (Vector3.zero in this case)
        
        if (agent != null)//checks for ai agent
        {
            if (impact.sqrMagnitude < 0.2f * 0.2f)//when the force returns to a low number
            {
                impact = Vector3.zero;//set impact to 0
                agent.enabled = true;//re enable the ai agent
            }
        }
    }
    public void AddForce(Vector3 force)
    {
        impact += force;
        if (agent != null)//checks for ai agent
        {
            agent.enabled = false;//disables ai agent to prevent fighting between force and ai nav
        }
    }

    public void Jump(float JumpForce)
    {
        verticalVelocity += JumpForce;
    }

    public void Reset()//resets players velocity
    {
        impact = Vector3.zero;
        verticalVelocity = 0f;
    }
}
