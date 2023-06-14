using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; } //set refrence to enemy animator in inspector
    [field: SerializeField] public CharacterController Controller { get; private set; }//gets the character controller component
    [field: SerializeField] public ForceReciever ForceReciever { get; private set; }//gets the force Reciever component
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }//gets the nav mesh component on the enemy
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }//gets the damage on the weapon
    [field: SerializeField] public Health Health { get; private set; }//gets refrence to the health component
    [field: SerializeField] public Target Target { get; private set; }//gets refrence to the target component
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }//gets refrence to the ragdoll component
    [field: SerializeField] public float PlayerChasingRange { get; private set; }//set the range that the enemy will detect the player
    [field: SerializeField] public float MovementSpeed { get; private set; }//speed the enemy will move at
    [field: SerializeField] public float AttackRange { get; private set; }//set the range that the enemy will detect the player
    [field: SerializeField] public int AttackDamage { get; private set; }//set the range that the enemy will detect the player
    [field: SerializeField] public float Knockback { get; private set; }//set the amount of knockback applied to attack

    public Health Player { get; private set; }//get the health component of the player, still allows us to get the location of the player.

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>(); //refrencing the transform of a component will get
                                                                                    //the transform of the object its attached to
        Agent.updatePosition = false;//dont want the agent to update position for us.
        Agent.updateRotation = false;//dont want the agent to update rotation for us.

        SwitchState(new EnemyIdleState(this));
    }

    private void OnEnable()//allows the switch to the desired state upon action, regardless of current state
    {
        Health.OnTakeDamage += HandleTakeDamage;//subscribes to the HandleTakeDamage method
        Health.OnDie += HandleDie;//subscribes to the HandleDie method
    }
    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;//unsubscribes to the HandleTakeDamage method
        Health.OnDie += HandleDie;// unsubscribes to the HandleDie method
    }
    private void HandleTakeDamage()
    {
        SwitchState(new EnemyImpactState(this));//switches to the impact state
    }
    private void HandleDie()
    {
        SwitchState(new EnemyDeadState(this));//switches to the impact state
    }
    private void OnDrawGizmosSelected()//draws a wireframe sphere in the editor only to show detection range
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
    }
}
