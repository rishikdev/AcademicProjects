using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LobbyInteraction : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gun;
    [SerializeField] private AnimationStateController animationStateController;
    [SerializeField] private GameObject playerHealthBar;
    [SerializeField] private ShootingBuildingInteraction shootingArena;
    [SerializeField] private TextMeshPro desiredEnemyCountScreen;
    [SerializeField] private Transform arenaEntryDoor;
    [SerializeField] private Transform mainDoor;

    private Transform character;
    private GameObject leftHandSpot;
    private GameObject rightHandSpot;
    private Rig leftHandRig;
    private Rig rightHandRig;
    private PlayerFire playerFire;
    private PlayerHealth playerHealth;
    private PlayerHealthBar healthBar;
    private Vector3 arenaDoorOriginalPosition;
    private Vector3 arenaDoorDesiredPosition;
    private Collider arenaEntryDoorCollider;
    private Vector3 mainDoorOriginalPosition;
    private Vector3 mainDoorDesiredPosition;
    private Collider mainDoorCollider;
    private AudioSource doorAudio;

    public bool openArenaDoor;
    public bool closeArenaDoor;
    public bool openMainDoor;
    public bool closeMainDoor;
    public bool isPlayerInShootingBuilding;
    public bool isPlayerInLobby;
    
    private void Awake()
    {
        player = GameObject.Find(Properties.PLAYER_GAMEOBJECT_NAME);
        character = player.transform.GetChild(1);
        gun = player.transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        playerFire = player.GetComponent<PlayerFire>();
        playerHealth = player.GetComponent<PlayerHealth>();
        playerHealthBar = player.transform.GetChild(3).GetChild(0).gameObject;
        healthBar = playerHealthBar.GetComponent<PlayerHealthBar>();

        leftHandSpot = gun.transform.GetChild(7).gameObject;
        rightHandSpot = gun.transform.GetChild(8).gameObject;

        leftHandRig = player.transform.GetChild(1).GetChild(0).GetChild(10).GetComponent<Rig>();
        rightHandRig = player.transform.GetChild(1).GetChild(0).GetChild(11).GetComponent<Rig>();

        animationStateController = character.GetChild(0).GetComponent<AnimationStateController>();

        desiredEnemyCountScreen.SetText(PlayerPrefs.GetInt(Properties.DESIRED_ENEMY_COUNT).ToString());
        shootingArena.desiredEnemyCount = PlayerPrefs.GetInt(Properties.DESIRED_ENEMY_COUNT);

        arenaDoorOriginalPosition = arenaEntryDoor.position;
        arenaDoorDesiredPosition = new Vector3(arenaDoorOriginalPosition.x, arenaDoorOriginalPosition.y + 3.75f, arenaDoorOriginalPosition.z);
        arenaEntryDoorCollider = arenaEntryDoor.GetComponent<BoxCollider>();

        mainDoorOriginalPosition = mainDoor.position;
        mainDoorDesiredPosition = new Vector3(mainDoorOriginalPosition.x, mainDoorOriginalPosition.y + 3.75f, mainDoorOriginalPosition.z);
        mainDoorCollider = mainDoor.GetComponent<BoxCollider>();

        doorAudio = GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        if (openArenaDoor)
            OpenDoor(arenaEntryDoorCollider, arenaEntryDoor, arenaDoorDesiredPosition);

        if (closeArenaDoor)
            CloseDoor(arenaEntryDoorCollider, arenaEntryDoor, arenaDoorOriginalPosition);

        if (openMainDoor)
            OpenDoor(mainDoorCollider, mainDoor, mainDoorDesiredPosition);

        if (closeMainDoor)
            CloseDoor(mainDoorCollider, mainDoor, mainDoorOriginalPosition);
    }

    // Player entered the lobby
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag(Properties.PLAYER_GAMEOBJECT_NAME))
            isPlayerInLobby = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Properties.PLAYER_GAMEOBJECT_NAME))
            DeactivatePlayerShootingControls();
    }

    // Player has exited the lobby
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Properties.PLAYER_GAMEOBJECT_NAME))
        {
            isPlayerInLobby = false;

            // Player has entered the arena
            if (shootingArena.isPlayerInShootingBuilding)
                ActivatePlayerShootingControls();

            // Player has left the building
            else
                DeactivatePlayerShootingControls();
        }
    }

    public void ActivatePlayerShootingControls()
    {
        playerHealthBar.SetActive(true);

        playerFire.enabled = true;
        animationStateController.isInShootingArena = true;
        animationStateController.leftHandSpot = leftHandSpot;
        animationStateController.rightHandSpot = rightHandSpot;

        gun.SetActive(true);
        leftHandRig.weight = 1;
        rightHandRig.weight = 1;
        shootingArena.InstantiateDesiredNumberOfEnemies();
    }

    public void DeactivatePlayerShootingControls()
    {
        playerHealthBar.SetActive(false);
        playerHealth.currentHealth = playerHealth.maxHealth;
        healthBar.SetHealthBarPercentage(1.0f);
        
        animationStateController.isInShootingArena = false;
        playerFire.enabled = false;

        gun.SetActive(false);
        leftHandRig.weight = 0;
        rightHandRig.weight = 0;

        if (shootingArena.currentEnemyCount > 0)
            shootingArena.DestroyAllEnemies();
    }

    private void OpenDoor(Collider doorCollider, Transform doorTransform, Vector3 doorDesiredPosition)
    {
        doorCollider.enabled = true;
        doorTransform.position = Vector3.Lerp(doorTransform.position, doorDesiredPosition, Time.deltaTime * 3.0f);

        if (doorTransform.position == doorDesiredPosition)
        {
            openMainDoor = false;
            openArenaDoor = false;
        }
    }

    private void CloseDoor(Collider doorCollider, Transform doorTransform, Vector3 doorOriginalPosition)
    {
        if (openArenaDoor || openMainDoor)
            return;

        doorCollider.enabled = false;
        doorTransform.position = Vector3.Lerp(doorTransform.position, doorOriginalPosition, Time.deltaTime * 3.0f);

        if (doorTransform.position == doorOriginalPosition)
        {
            closeMainDoor = false;
            closeArenaDoor = false;
        }
    }

    public void PlayDoorSound()
    {
        doorAudio.Play();
    }
}
