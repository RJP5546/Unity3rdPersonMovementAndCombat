using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{

    [SerializeField] private Collider myCollider;//set your object to prevent self hit
    
    private List<Collider> alreadyCollidedWith = new List<Collider>();
    private int damage;//the amount of damage the weapon does
    private float knockback;//the amount of knockback the weapon does

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other == myCollider) { return; } //dont hit yourself

        if(alreadyCollidedWith.Contains(other)) { return; } //if previously collided with, ignore

        alreadyCollidedWith.Add(other);//if not previously collided with, add to list

        if(other.TryGetComponent<Health>(out Health health))//if the other object has the health component
        {
            health.DealDamage(damage);//damage actually dealt
        }

        if(other.TryGetComponent<ForceReciever>(out  ForceReciever forceReciever))
        {
            Vector3 direction = (other.transform.position - myCollider.transform.position).normalized; //gets vector between player and enemy
            forceReciever.AddForce( direction * knockback); //applies the force in the proper direction
        }
    }

    public void SetAttack(int damage, float knockback)
    {
        this.damage = damage;//sets the incoming damage value as the attacks damage
        this.knockback = knockback;//sets the incoming knockback value as the attacks knockback
    }
}
