using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator animator;//refrence to the animator so we can disable it to prevent weird things
    [SerializeField] private CharacterController controller;//disable the character controller to remove
                                                            //the normal colliders to use the radgoll colliders

    private Collider[] allColliders;//a list of all the normal colliders
    private Rigidbody[] allRigidbodies;//a list of all the normal rigidbodies
    private void Start()
    {
        allColliders = GetComponentsInChildren<Collider>(true);//will get all game objects regardless of if they are enabled or not.
        allRigidbodies = GetComponentsInChildren<Rigidbody>(true);//These is a very expensive method and will only be called once upon start.
        ToggleRagdoll(false);//upon startup, set the ragdoll to false.    
    }

    public void ToggleRagdoll(bool isRagdoll)//A public method to be called elsewhere to toggle the ragdoll state
    {
        foreach(Collider collider in allColliders)
        {
            if (collider.gameObject.CompareTag("Ragdoll"))//cehcks to see if the collider in collider[] has the ragdoll tag
            {
                collider.enabled = isRagdoll;//sets the ragdoll tagged collider to on or off
            }
        }
        foreach (Rigidbody rigidbody in allRigidbodies)
        {
            if (rigidbody.gameObject.CompareTag("Ragdoll"))//cehcks to see if the collider in collider[] has the ragdoll tag
            {
                rigidbody.isKinematic = !isRagdoll;//sets if the object uses physics or not. opposite to the value of isRagdoll
                rigidbody.useGravity = isRagdoll;//sets if the object uses gravity or not. same value as isRagdoll
            }
        }

        controller.enabled = !isRagdoll;//Controller is enabled if ragdoll is false, or disabled if ragdolling
        animator.enabled = !isRagdoll;//Animator is enabled if ragdoll is false, or disabled if ragdolling
    }
}
