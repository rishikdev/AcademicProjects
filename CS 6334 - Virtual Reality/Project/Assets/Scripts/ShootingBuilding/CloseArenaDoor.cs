using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseArenaDoor : MonoBehaviour
{
    [SerializeField] private LobbyInteraction lobby;
    [SerializeField] private ShootingBuildingInteraction shootingArena;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == Properties.PLAYER_GAMEOBJECT_NAME)
        {
            lobby.openArenaDoor = false;
            lobby.closeArenaDoor = true;
            lobby.PlayDoorSound();
        }
    }
}
