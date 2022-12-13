using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.EventSystems;

public class CarInteraction : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject driverSeat;
    [SerializeField] private GameObject leftHandSpot;
    [SerializeField] private GameObject rightHandSpot;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private AnimationStateController animationStateController;

    public bool isPlayerInCar;

    private Transform inCarCameraPosition;
    private Transform mainCamera;
    private Transform mainCameraParent;
    private Transform character;

    private Rig leftHandRig;
    private Rig rightHandRig;
    private Rig leftLegRig;
    private Rig rightLegRig;

    private CarController carController;
    private PlayerMovement playerMovement;
    private CharacterRotate characterRotate;
    private CapsuleCollider playerCapsuleCollider;

    private EventTrigger eventTrigger;

    private void Awake()
    {
        player = GameObject.Find(Properties.PLAYER_GAMEOBJECT_NAME);
        playerRigidbody = player.GetComponent<Rigidbody>();
        character = player.transform.GetChild(1);

        leftHandRig = player.transform.GetChild(1).GetChild(0).GetChild(10).GetComponent<Rig>();
        rightHandRig = player.transform.GetChild(1).GetChild(0).GetChild(11).GetComponent<Rig>();
        leftLegRig = player.transform.GetChild(1).GetChild(0).GetChild(12).GetComponent<Rig>();
        rightLegRig = player.transform.GetChild(1).GetChild(0).GetChild(13).GetComponent<Rig>();

        leftHandSpot = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).gameObject;
        rightHandSpot = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).gameObject;

        mainCameraParent = player.transform.GetChild(0);
        inCarCameraPosition = transform.GetChild(3);
        mainCamera = mainCameraParent.GetChild(0);

        carController = gameObject.GetComponent<CarController>();
        playerMovement = player.GetComponent<PlayerMovement>();
        characterRotate = character.GetComponent<CharacterRotate>();
        playerCapsuleCollider = player.GetComponent<CapsuleCollider>();

        eventTrigger = gameObject.GetComponent<EventTrigger>();
        eventTrigger.enabled = false;

        animationStateController = character.GetChild(0).GetComponent<AnimationStateController>();
    }

    private void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= 10.0f)
            eventTrigger.enabled = true;
        else
            eventTrigger.enabled = false;
    }

    public void OnPointerClick()
    {
        // The player wants to get out of the car
        if(isPlayerInCar)
        {
            isPlayerInCar = false;

            leftHandRig.weight = 0;
            rightHandRig.weight = 0;

            leftLegRig.weight = 0;
            rightLegRig.weight = 0;

            carController.enabled = false;

            player.transform.parent = null;
            playerRigidbody.isKinematic = false;
            playerCapsuleCollider.enabled = true;
            playerMovement.enabled = true;

            characterRotate.enabled = true;
            player.transform.localEulerAngles = new Vector3(0, player.transform.localEulerAngles.y, 0);
            player.transform.localScale = new Vector3(1, 1, 1);
            animationStateController.isDriving = false;
            mainCamera.gameObject.GetComponent<Camera>().nearClipPlane = 0.35f;
            mainCamera.SetParent(mainCameraParent);
        }

        // The player wants to get in the car
        else
        {
            animationStateController.leftHandSpot = leftHandSpot;
            animationStateController.rightHandSpot = rightHandSpot;

            isPlayerInCar = true;

            leftHandRig.weight = 1;
            rightHandRig.weight = 1;

            leftLegRig.weight = 1;
            rightLegRig.weight = 1;

            carController.enabled = true;

            playerCapsuleCollider.enabled = false;
            playerMovement.enabled = false;

            character.eulerAngles = transform.eulerAngles;
            characterRotate.enabled = false;

            player.transform.parent = gameObject.transform;
            player.transform.position = new Vector3(driverSeat.transform.position.x, driverSeat.transform.position.y + 1.25f, driverSeat.transform.position.z);

            playerRigidbody.isKinematic = true;

            animationStateController.isDriving = true;
            mainCamera.gameObject.GetComponent<Camera>().nearClipPlane = 0.2f;
            mainCamera.SetParent(inCarCameraPosition);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.name == Properties.PLAYER_GAMEOBJECT_NAME)
        //    eventTrigger.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.name == Properties.PLAYER_GAMEOBJECT_NAME)
        //    eventTrigger.enabled = false;
    }
}
