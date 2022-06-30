using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;
    public Transform leftIndexTip;
    public Transform leftThumbTip;
    public Transform rightIndexTip;
    public Transform rightThumbTip;
    public GameObject hand;

    private Client client;
    private Rigidbody thisRigidbody;
    private InteractionProperties interactionProperties;
    private Client.HandPose currentHandPose;
    private LeftHand leftHandScript;
    private RightHand rightHandScript;

    private Outline outline;

    private float rotationAndScaleSpeed = 10;
    private bool rotationAndScaleOriginSet;
    private Vector3 rotationAndScaleOrigin;
    private bool isInteractionOn;

    // Start is called before the first frame update
    void Start()
    {
        client = FindObjectOfType<Client>();
        thisRigidbody = gameObject.GetComponent<Rigidbody>();
        interactionProperties = FindObjectOfType<InteractionProperties>();
        currentHandPose = Client.HandPose.None;
        leftHandScript = leftHand.GetComponent<LeftHand>();
        rightHandScript = rightHand.GetComponent<RightHand>();

        outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hand != null)
        {
            currentHandPose = (hand == leftHand) ? client.leftHandPose : client.rightHandPose;

            if (currentHandPose == Client.HandPose.OK)
            {
                if (interactionProperties.interaction == InteractionProperties.Interaction.Move)
                {
                    isInteractionOn = true;
                    Move();
                }

                else if (interactionProperties.interaction == InteractionProperties.Interaction.Rotate)
                {
                    isInteractionOn = true;
                    Rotate();
                }

                else if (interactionProperties.interaction == InteractionProperties.Interaction.Scale)
                {
                    isInteractionOn = true;
                    Scale();
                }

                else
                {
                    isInteractionOn = false;
                }
            }

            else if(currentHandPose == Client.HandPose.Stop)
            {
                rotationAndScaleOriginSet = false;

                if (hand == leftHand)
                {
                    if (leftHandScript.currentInteractableObject == gameObject)
                    {
                        leftHandScript.currentInteractableObject = null;
                        hand = null;
                        thisRigidbody.isKinematic = false;
                        outline.enabled = false;
                    }
                }

                else if (hand == rightHand)
                {
                    if (rightHandScript.currentInteractableObject == gameObject)
                    {
                        rightHandScript.currentInteractableObject = null;
                        hand = null;
                        thisRigidbody.isKinematic = false;
                        outline.enabled = false;
                    }
                }
            }
        }
    }

    private void Move()
    {
        Vector3 thumbPosition = (hand == leftHand) ? leftThumbTip.position : rightThumbTip.position;
        Vector3 indexPosition = (hand == leftHand) ? leftIndexTip.position : rightIndexTip.position;
        Vector3 position = (thumbPosition + indexPosition) / 2;

        transform.position = position;
    }

    private void Rotate()
    {
        Vector3 thumbPosition = (hand == leftHand) ? leftThumbTip.position : rightThumbTip.position;
        Vector3 indexPosition = (hand == leftHand) ? leftIndexTip.position : rightIndexTip.position;
        Vector3 position = (thumbPosition + indexPosition) / 2;

        if (!rotationAndScaleOriginSet)
        {
            rotationAndScaleOriginSet = true;
            rotationAndScaleOrigin = position;
        }

        float deltaX = position.x - rotationAndScaleOrigin.x;
        float deltaY = position.y - rotationAndScaleOrigin.y;
        float deltaZ = position.z - rotationAndScaleOrigin.z;

        Vector3 rotation = Vector3.Scale(interactionProperties.axes, new Vector3(deltaZ, -deltaX, deltaY));
        transform.eulerAngles = transform.eulerAngles + rotation * rotationAndScaleSpeed;

        rotationAndScaleOrigin = position;
    }

    private void Scale()
    {
        Vector3 thumbPosition = (hand == leftHand) ? leftThumbTip.position : rightThumbTip.position;
        Vector3 indexPosition = (hand == leftHand) ? leftIndexTip.position : rightIndexTip.position;
        Vector3 position = (thumbPosition + indexPosition) / 2;

        if (!rotationAndScaleOriginSet)
        {
            rotationAndScaleOriginSet = true;
            rotationAndScaleOrigin = position;
        }

        float deltaX = position.x - rotationAndScaleOrigin.x;
        float deltaY = position.y - rotationAndScaleOrigin.y;
        float deltaZ = position.z - rotationAndScaleOrigin.z;

        Vector3 scale = Vector3.Scale(interactionProperties.axes, new Vector3(deltaX, deltaY, deltaZ));
        transform.localScale = transform.localScale + scale;

        rotationAndScaleOrigin = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Left Hand")
        {
            outline.enabled = true;

            if (leftHandScript.currentInteractableObject == null)
            {
                leftHandScript.currentInteractableObject = gameObject;
                hand = leftHand;
                thisRigidbody.isKinematic = true;
            }
        }

        else if (other.name == "Right Hand")
        {
            outline.enabled = true;

            if (rightHandScript.currentInteractableObject == null)
            {
                rightHandScript.currentInteractableObject = gameObject;
                hand = rightHand;
                thisRigidbody.isKinematic = true;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!isInteractionOn)
            outline.enabled = false;
    }
}