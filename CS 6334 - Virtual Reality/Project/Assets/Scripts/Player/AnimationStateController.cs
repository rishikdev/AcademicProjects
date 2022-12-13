using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 2f;
    [SerializeField] private float velocityX = 0f;
    [SerializeField] private float velocityZ = 0f;
    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private Transform rightHandTarget;

    public GameObject leftHandSpot;
    public GameObject rightHandSpot;

    public bool isDriving;
    public bool isInShootingArena;

    private float moveDirectionX;
    private float moveDirectionY;

    private bool isIdle;
    private bool isMovingVertical;
    private bool isMovingHorizontal;

    private void Start()
    {
        leftHandSpot = null;
        rightHandSpot = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDriving)
        {
            animator.SetBool(Properties.IS_DRIVING, true);
            animator.SetBool(Properties.IS_IN_SHOOTING_ARENA, false);

            leftHandTarget.position = leftHandSpot.transform.position;
            rightHandTarget.position = rightHandSpot.transform.position;
        }

        if(isInShootingArena)
        {
            animator.SetBool(Properties.IS_DRIVING, false);
            animator.SetBool(Properties.IS_IN_SHOOTING_ARENA, true);

            leftHandTarget.position = leftHandSpot.transform.position;
            rightHandTarget.position = rightHandSpot.transform.position;

            MovementAnimation();
        }

        if(!isDriving && !isInShootingArena)
        {
            animator.SetBool(Properties.IS_DRIVING, false);
            animator.SetBool(Properties.IS_IN_SHOOTING_ARENA, false);
            MovementAnimation();
        }
    }

    private void MovementAnimation()
    {
        moveDirectionX = playerMovement.moveDirection.x;
        moveDirectionY = playerMovement.moveDirection.y;

        isIdle = playerMovement.moveDirection.magnitude < 0.1f;
        isMovingVertical = Mathf.Abs(moveDirectionY) > 0.1f;
        isMovingHorizontal = Math.Abs(moveDirectionX) > 0.1f;

        if (isIdle)
        {
            DecrementVelocityZ();
            DecrementVelocityX();
        }

        else
        {
            if (isMovingVertical)
                velocityZ = Mathf.Clamp(velocityZ + Time.deltaTime * moveDirectionY * acceleration, -1.0f, 1.0f);

            else
                DecrementVelocityZ();

            if (isMovingHorizontal)
                velocityX = Mathf.Clamp(velocityX + Time.deltaTime * moveDirectionX * acceleration, -1.0f, 1.0f);

            else
                DecrementVelocityX();
        }

        animator.SetFloat(Properties.VELOCITY_X, velocityX);
        animator.SetFloat(Properties.VELOCITY_Z, velocityZ);
    }

    private void DecrementVelocityZ()
    {
        if (velocityZ >= 0.05f)
            velocityZ = velocityZ - Time.deltaTime * deceleration;

        else if (velocityZ <= -0.05f)
            velocityZ = velocityZ + Time.deltaTime * acceleration;

        else
            velocityZ = 0f;
    }

    private void DecrementVelocityX()
    {
        if (velocityX >= 0.05f)
            velocityX = velocityX - Time.deltaTime * deceleration;

        else if (velocityX <= -0.05f)
            velocityX = velocityX + Time.deltaTime * acceleration;

        else
            velocityX = 0f;
    }
}
