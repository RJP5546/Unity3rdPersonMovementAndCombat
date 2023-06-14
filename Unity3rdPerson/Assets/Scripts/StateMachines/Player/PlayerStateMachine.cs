using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader {get; private set; }//set the players input reader component in the inspector
    [field: SerializeField] public CharacterController Controller { get; private set; }//set the players Character controller component in the inspector
    [field: SerializeField] public Animator Animator { get; private set; }//set the players animator component in the inspector
    [field: SerializeField] public Targeter Targeter { get; private set; }//set the players targeter component in the inspector
    [field: SerializeField] public ForceReciever ForceReciever { get; private set; }//set the players force reciever component in the inspector
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }//set the players weaponLogic component in the inspector
    [field: SerializeField] public Health Health { get; private set; }//set the players health component in the inspector
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }//set the players ragdoll component in the inspector
    [field: SerializeField] public LedgeDetector LedgeDetector { get; private set; }//set the ledge detector component in the inspector
    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }//set the players free look movement speed value
    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }//set the players targeting movement speed value
    [field: SerializeField] public float RotationDampening { get; private set; }
    [field: SerializeField] public float DodgeDuration { get; private set; }//how long the dodge lasts
    [field: SerializeField] public float DodgeLength { get; private set; }//How far the player moves
    [field: SerializeField] public float JumpForce { get; private set; }//Force to apply for jump
    [field: SerializeField] public Attack[] Attacks { get; private set; }//Array of attacks



    public float PreviousDodgeTime { get; private set; } = Mathf.NegativeInfinity;//Time that the previous dodge was performed
    public Transform MainCameraTransform { get; private set; }//transform location of the main cam

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//locks the cursor to the center of the screen
        Cursor.visible = false;//makes the cursor invisible ingame
        MainCameraTransform = Camera.main.transform;//sets the main camera transform
        SwitchState(new PlayerFreeLookState(this));//upon start, puts player into the free look state
    }
    private void OnEnable()//allows the switch to the desired state upon action, regardless of current state
    {
        Health.OnTakeDamage += HandleTakeDamage;//subscribes to the HandleTakeDamage method
        Health.OnDie += HandleDie;//subscribes to the HandleDie method
    }
    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;//unsubscribes to the HandleTakeDamage method
        Health.OnDie -= HandleDie;// unsubscribes to the HandleDie method
    }
    private void HandleTakeDamage()
    {
        SwitchState(new PlayerImpactState(this));//switches to the impact state
    }
    private void HandleDie()
    {
        SwitchState(new PlayerDeadState(this));//switches to the impact state
    }

}
