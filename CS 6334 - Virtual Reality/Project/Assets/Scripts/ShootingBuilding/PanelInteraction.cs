using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelInteraction : MonoBehaviour
{
    [SerializeField] private ShootingBuildingInteraction shootingArena;
    [SerializeField] private LobbyInteraction lobby;
    [SerializeField] private TextMeshPro desiredEnemyCountScreen;

    // Start is called before the first frame update
    void Start()
    {
        desiredEnemyCountScreen.SetText(PlayerPrefs.GetInt(Properties.DESIRED_ENEMY_COUNT).ToString());
        shootingArena.desiredEnemyCount = PlayerPrefs.GetInt(Properties.DESIRED_ENEMY_COUNT);
    }

    public void OnPointerClick(GameObject obj)
    {
        if (obj.name == Properties.BUTTON_DECREMENT_GAMEOBJECT_NAME)
        {
            shootingArena.UpdateDesiredEnemyCount(-1);
            PlayerPrefs.SetInt(Properties.DESIRED_ENEMY_COUNT, shootingArena.desiredEnemyCount);
            desiredEnemyCountScreen.SetText(shootingArena.desiredEnemyCount.ToString());
        }

        if (obj.name == Properties.BUTTON_INCREMENT_GAMEOBJECT_NAME)
        {
            shootingArena.UpdateDesiredEnemyCount(1);
            PlayerPrefs.SetInt(Properties.DESIRED_ENEMY_COUNT, shootingArena.desiredEnemyCount);
            desiredEnemyCountScreen.SetText(shootingArena.desiredEnemyCount.ToString());
        }

        if (obj.name == Properties.BUTTON_ARENA_ENTER_GAMEOBJECT_NAME)
        {
            PlayerPrefs.SetInt(Properties.DESIRED_ENEMY_COUNT, shootingArena.desiredEnemyCount);
            lobby.PlayDoorSound();
            lobby.openArenaDoor = true;
        }

        if (obj.name.Equals(Properties.BUTTON_ARENA_EXIT_GAMEOBJECT_NAME))
        {
            lobby.PlayDoorSound();
            lobby.openArenaDoor = true;
        }

        if(obj.name.Equals(Properties.BUTTON_BUILDING_ENTER_GAMEOBJECT_NAME))
        {
            lobby.PlayDoorSound();
            lobby.openMainDoor = true;
        }

        if (obj.name.Equals(Properties.BUTTON_BUILDING_EXIT_GAMEOBJECT_NAME))
        {
            lobby.PlayDoorSound();
            lobby.openMainDoor = true;
        }
    }
}
