using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public abstract class State
{
    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();

    protected float GetNormalizedTime(Animator animator, string tag)
    //any player or enemy has its own state machine, so instead of refrencing animator from the state machine, pass the animator up to
    //the class and store it here as a temp var. Also pass up animation tags to return normalized time to
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0); //get current animator state info, for layer 0
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0); //get the next animator state info, for layer 0

        if (animator.IsInTransition(0) && nextInfo.IsTag(tag)) //Attack tag assigned in animator
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag)) //Not transitioning and currenty playing attacking anim
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
