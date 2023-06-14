using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cineTargetGroup;

    private Camera mainCamera;

    private List<Target> targets = new List<Target>();
    public Target CurrentTarget { get; private set; }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }
        targets.Add(target);
        target.OnDestroyed += RemoveTarget;
    }

    private void OnTriggerExit(Collider other)
    {
        //Same as code above, just reduced line count. Tries to get component, if sucessful var is stored in the out function.
        if(!other.TryGetComponent<Target>(out Target target)) { return; }
        RemoveTarget(target);//calls remove target to remove the target

    }

    //Bool method to return if we have a target. Also stores index 0 as Current target
    public bool SelectTarget()
    {
        if (CurrentTarget != null) { cineTargetGroup.RemoveMember(CurrentTarget.transform); }//Prevents adding the same object twice
        if (targets.Count == 0) { return false; }//no targets to select, ignore
        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;
        foreach(Target target in targets) // Goes through every item in Target list
        {
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);//gets a vector 2 between tharget and camera
            if(!target.GetComponentInChildren<Renderer>().isVisible)//checks the renderer component of the target to see if it is on screen,
                                                                   //you need to use component in children so that it will keep searching,
                                                                   //the object and its children until it finds a renderer
            {
                continue;
            }

            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);//find middle point on screen
            if(toCenter.sqrMagnitude < closestTargetDistance) //sqr magnitude is faster for system to check, if the new one is smaller, its closer
            {
                closestTarget = target;//update new closest target
                closestTargetDistance = toCenter.sqrMagnitude; //set the new distance
            }
        }

        if (closestTarget == null) { return false; } //if there are no targets do not enter targeting state.

        CurrentTarget = closestTarget; //sets the closest target to the current
        cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
        return true;
    }

    public void Cancel()
    {
        if (CurrentTarget == null) { return; }

        cineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    private void RemoveTarget(Target target)
    {
        
        if (CurrentTarget == target)//if the current target is the target being removed:
        {
            cineTargetGroup.RemoveMember(CurrentTarget.transform);//remove the camera component
            CurrentTarget = null;//sets no current target
        }

        target.OnDestroyed -= RemoveTarget;//unsub form the method
        targets.Remove(target);//remove the target
    }

}
