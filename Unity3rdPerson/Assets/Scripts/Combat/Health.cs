using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int health;
    private bool isInvulnerable = false;

    public event Action OnTakeDamage;
    public event Action OnDie;

    public bool IsDead => health == 0;//a public bool that if true will return health = 0.

    private void Start()
    {
        health = maxHealth;  
    }

    public void DealDamage(int damage)
    {
        if (health <= 0) { return; }//if health is less or equal to 0, ignore damage
        if(isInvulnerable) { return; }//if invulnerable ignore damage
        health = Mathf.Max(health -damage, 0); //sets the larger number. Either the remaining health or 0. Dont have to worry about neg.
        OnTakeDamage?.Invoke();//adding?is a null check. lets listeners know we took damage.
        if (health == 0)
        {
            OnDie?.Invoke();
        }
        Debug.Log(health);
    }

    public void SetInvulnerable(bool isInvunerable)//sets if we want the object to not take any damage
    {
        this.isInvulnerable = isInvunerable;
    }
}
