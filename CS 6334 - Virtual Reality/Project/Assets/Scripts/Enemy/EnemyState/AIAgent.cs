using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    [Header("Agent")]
    public AIStateMachine stateMachine;
    public AIStateId initialState;
    public NavMeshAgent navMeshAgent;
    public AIAgentConfig config;
    public AISensor sensor;

    [Header("Ragdoll And Health")]
    public Ragdoll ragdoll;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public EnemyHealthBar healthBar;
    public EnemyHealth health;
    public Transform deathTransform;

    [Header("Weapon")]
    public WeaponIK weaponIK;
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer bulletTracer;
    public AudioSource audioSource;
    public Transform raycastOrigin;
    //public Transform raycastDestination;

    [Header("AI")]
    public bool isHit;
    public ShootingBuildingInteraction shootingBuilding;
    public Transform[] waypoints;
    private int i = 0;
    public Vector3 lastShootingSoundTransform;

    [Header("Player")]
    public Transform playerTransform;
    public PlayerHealth playerHealth;
    public PlayerFire playerFire;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        sensor = GetComponent<AISensor>();

        ragdoll = GetComponent<Ragdoll>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();

        weaponIK = GetComponent<WeaponIK>();
        bulletTracer = GameObject.Find(Properties.BULLET_TRACER_GAMEOBJECT_NAME).GetComponent<TrailRenderer>();
        audioSource = GetComponent<AudioSource>();

        shootingBuilding = GameObject.Find(Properties.SHOOTING_BUILDING_GAMEOBJECT_NAME).GetComponent<ShootingBuildingInteraction>();
        foreach(Transform waypointTransform in shootingBuilding.gameObject.transform.FindChild(Properties.WAYPOINTS_GAMEOBJECT_NAME))
        {
            waypoints[i++] = waypointTransform;
        }
        lastShootingSoundTransform = shootingBuilding.lastFirePosition;

        playerTransform = GameObject.FindGameObjectWithTag(Properties.PLAYER_GAMEOBJECT_NAME).transform;
        playerHealth = playerTransform.gameObject.GetComponent<PlayerHealth>();
        playerFire = playerTransform.gameObject.GetComponent<PlayerFire>();

        stateMachine = new AIStateMachine(this);

        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.RegisterState(new AIIdleState());

        stateMachine.ChangeState(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        lastShootingSoundTransform = shootingBuilding.lastFirePosition;

        stateMachine.Update();
    }
}
