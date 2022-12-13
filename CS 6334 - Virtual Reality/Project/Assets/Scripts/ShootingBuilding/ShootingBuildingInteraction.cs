using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBuildingInteraction : MonoBehaviour
{
    [SerializeField] private LobbyInteraction playerLobbyInteraction;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Material playerFadePlaneMaterial;
    [SerializeField] private GameObject gun;
    [SerializeField] private LobbyInteraction lobby;

    public int desiredEnemyCount;
    public int currentEnemyCount;
    public bool isPlayerDead;
    public bool isPlayerInShootingBuilding;

    private float playerFadeInTimer;
    private float playerFadeOutTimer;
    private float playerFadeInTime = 0.0f;
    private float playerFadeOutTime = 1.0f;
    private GameObject player;
    private PlayerFire playerFire;
    private PlayerMovement playerMovement;
    private AudioSource enemySpawnAudio;

    private void Awake()
    {
        player = GameObject.Find(Properties.PLAYER_GAMEOBJECT_NAME);
        playerFire = player.GetComponent<PlayerFire>();
        playerMovement = player.GetComponent<PlayerMovement>();

        playerFadeInTimer = playerFadeInTime;
        playerFadeOutTimer = playerFadeOutTime;
        enemySpawnAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isPlayerDead)
        {
            if (playerFadeInTimer <= playerFadeOutTime)
                FadeInPlayerFadePlane();

            else
            {
                if (playerFadeOutTimer >= playerFadeOutTime)
                    InstantiatePlayer();

                else
                    FadeOutPlayerFadePlane();

                playerFadeOutTimer = playerFadeOutTimer - Time.deltaTime;
            }

            playerFadeInTimer = playerFadeInTimer + Time.deltaTime;
        }

        else
        {
            playerFadeInTimer = playerFadeInTime;
            playerFadeOutTimer = playerFadeOutTime;
        }
    }

    public void UpdateCurrentEnemyCount(int count)
    {
        currentEnemyCount = currentEnemyCount + count;
        currentEnemyCount = Mathf.Clamp(currentEnemyCount, 0, desiredEnemyCount);
    }

    public void UpdateDesiredEnemyCount(int amount)
    {
        desiredEnemyCount = desiredEnemyCount + amount;
        desiredEnemyCount = Mathf.Clamp(desiredEnemyCount, 0, 10);
    }

    public Vector3 lastFirePosition;

    // Player has entered the shooting building
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag(Properties.PLAYER_GAMEOBJECT_NAME))
            isPlayerInShootingBuilding = true;
    }

    // Player has left the shooting building
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Properties.PLAYER_GAMEOBJECT_NAME))
        {
            isPlayerInShootingBuilding = false;
            lobby.DeactivatePlayerShootingControls();
        }
    }

    public void InstantiateDesiredNumberOfEnemies()
    {
        List<Vector3> occupiedPositions = new List<Vector3>();

        for (int i = currentEnemyCount; i < desiredEnemyCount; i = i + 1)
        {
            GameObject newEnemy = InstantiateOneEnemy();

            if (occupiedPositions.Contains(newEnemy.transform.position))
                DestroyOneEnemy(newEnemy);

            else
                occupiedPositions.Add(newEnemy.transform.position);
        }
    }

    public GameObject InstantiateOneEnemy()
    {
        Transform waypoint = waypoints[Random.Range(0, waypoints.Length)].transform;
        GameObject enemyClone = Instantiate(enemy, waypoint.transform);
        enemyClone.transform.SetParent(null);

        UpdateCurrentEnemyCount(1);
        enemySpawnAudio.Play();

        return enemyClone;
    }

    public void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Properties.ENEMY_GAMEOBJECT_NAME);

        foreach (GameObject obj in enemies)
        {
            Destroy(obj);
            UpdateCurrentEnemyCount(-1);
        }
    }

    public void DestroyOneEnemy(GameObject enemyClone)
    {
        Destroy(enemyClone);
        UpdateCurrentEnemyCount(-1);

        InstantiateOneEnemy();
    }

    public void InstantiatePlayer()
    {
        Vector3 newPosition = waypoints[Random.Range(0, waypoints.Length)].transform.position;
        player.transform.position = new Vector3(newPosition.x, newPosition.y + 1.8f, newPosition.z);
        player.transform.SetParent(null);
        playerFire.enabled = true;
        playerMovement.enabled = true;
    }

    public void FadeInPlayerFadePlane()
    {
        Color newColour = playerFadePlaneMaterial.color;

        newColour.a = newColour.a + 1 * Time.deltaTime;
        playerFadePlaneMaterial.color = newColour;
    }

    private void FadeOutPlayerFadePlane()
    {
        Color newColour = playerFadePlaneMaterial.color;

        newColour.a = newColour.a - 1 * Time.deltaTime;
        playerFadePlaneMaterial.color = newColour;

        if(newColour.a <= 0.0f)
        {
            isPlayerDead = false;
        }
    }
}
