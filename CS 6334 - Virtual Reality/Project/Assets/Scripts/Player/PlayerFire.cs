using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private PlayerInputAction playerInputAction;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private int fireRate = 10;
    [SerializeField] private AudioSource laserAudioSource;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private Transform raycastDestination;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private TrailRenderer bulletTracer;
    [SerializeField] private ShootingBuildingInteraction playerShootingBuildingInteraction;

    private InputAction fire;
    private GameObject gun;
    private bool isFiring;
    private float nextTimeToFire = 0f;
    private Ray ray;
    private RaycastHit hitInfo;

    public bool hasFired;
    public float damage = 10f;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();

        mainCamera = transform.GetChild(0).GetChild(0);
        gun = mainCamera.GetChild(1).gameObject;

        laserAudioSource = gun.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        fire = playerInputAction.Player.Fire;
        playerInputAction.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Player.Disable();
    }

    private void Update()
    {
        isFiring = fire.ReadValue<float>() == 1;

        if (isFiring && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Fire();
            hasFired = true;
            playerShootingBuildingInteraction.lastFirePosition = transform.position;
        }

        else
            hasFired = false;
    }

    private void Fire()
    {
        laserAudioSource.Play();
        muzzleFlash.Emit(1);

        ray.origin = raycastOrigin.position;
        ray.direction = raycastDestination.position -  raycastOrigin.position;

        var tracer = Instantiate(bulletTracer, ray.origin, Quaternion.identity);
        tracer.AddPosition(ray.origin);

        if(Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.transform.gameObject.layer != Properties.ENEMY_LAYER)
            {
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                hitEffect.Emit(1);
            }

            tracer.transform.position = hitInfo.point;

            var hitBox = hitInfo.collider.GetComponent<HitBox>();

            if (hitBox)
                hitBox.OnRaycastHit(this, ray.direction);
        }
    }
}
