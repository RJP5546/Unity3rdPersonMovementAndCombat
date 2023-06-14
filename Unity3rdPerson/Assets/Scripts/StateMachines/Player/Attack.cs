using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] //requires system and allows this calss to be seen in the editor.
public class Attack
{
    [field: SerializeField] public string AnimationName { get; private set; } //Serialise field makes it visable in editor
                                                                              //name is uppercase because it is a property
                                                                              //field is added to make it read only
    [field: SerializeField] public float TransitionDuration { get; private set; }
    [field: SerializeField] public int ComboStateIndex { get; private set; } = -1; //used to say when the end of the combo is
    [field: SerializeField] public float ComboAttackTime { get; private set; } //used to say how far into an animation before
                                                                               //you are allowed to do the next attack
    [field: SerializeField] public float ForceTime { get; private set; } //how far into the animation is the force applied
    [field: SerializeField] public float Force { get; private set; } //how much force to apply
    [field: SerializeField] public float KnockBack { get; private set; } //how much force to apply
    [field: SerializeField] public int Damage { get; private set; } //how much damage to apply
}
