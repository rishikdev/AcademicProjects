using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHand : MonoBehaviour
{
    private const float HAND_TRAVEL_DISTANCE = 3f;
    private const float MINIMUM_HAND_DISTANCE = 3;

    private Client client;
    private Landmarks landmarks;
    private Animator animator;
    private Client.HandPose handPose;

    public GameObject origin;
    public GameObject currentInteractableObject;

    private Vector3 originPosition;
    private Vector3 middleBasePosition;

    // Start is called before the first frame update
    void Start()
    {
        client = FindObjectOfType<Client>();
        landmarks = GetComponent<Landmarks>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        originPosition = landmarks.leftOriginPosition;
        originPosition.y = -originPosition.y;
        originPosition.z = transform.position.z;

        middleBasePosition = landmarks.leftMiddleBasePosition;
        middleBasePosition.y = -middleBasePosition.y;
        middleBasePosition.z = transform.position.z;

        ForwardBackward();
        RotateAlongX();
        SetPose();
    }

    private void ForwardBackward()
    {
        // Distance between origin and middleBase
        float verticalScale = Vector3.Distance(originPosition, middleBasePosition);

        // Move foreward and backward
        originPosition.z = Mathf.Max(HAND_TRAVEL_DISTANCE * verticalScale, MINIMUM_HAND_DISTANCE);
        origin.transform.position = originPosition;
    }

    private void RotateAlongX()
    {
        // Rotate about Z-axis
        Vector3 direction = middleBasePosition - originPosition;
        float angle = Vector3.SignedAngle(direction, Vector3.up, Vector3.forward);
        angle = Mathf.Clamp(angle, -90, 90);

        transform.eulerAngles = new Vector3(angle, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    private void SetPose()
    {
        handPose = client.leftHandPose;

        if(handPose == Client.HandPose.Fist && !animator.GetBool("isFist"))
        {
            animator.SetBool("isStop", false);
            animator.SetBool("isOk", false);
            animator.SetBool("isPointer", false);
            animator.SetBool("isFist", true);
        }

        else if(handPose == Client.HandPose.OK && !animator.GetBool("isOk"))
        {
            animator.SetBool("isFist", false);
            animator.SetBool("isStop", false);
            animator.SetBool("isPointer", false);
            animator.SetBool("isOk", true);
        }

        else if (handPose == Client.HandPose.Stop && !animator.GetBool("isStop"))
        {
            animator.SetBool("isFist", false);
            animator.SetBool("isPointer", false);
            animator.SetBool("isOk", false);
            animator.SetBool("isStop", true);
        }

        else if (handPose == Client.HandPose.Point && !animator.GetBool("isPointer"))
        {
            animator.SetBool("isFist", false);
            animator.SetBool("isStop", false);
            animator.SetBool("isOk", false);
            animator.SetBool("isPointer", true);
        }
    }
}