using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputAction playerInputAction;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float avatarHeight = 1.75f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private InputAction move;

    public Vector2 moveDirection;
    private Vector3 forceDirection;
    private Vector3 horizontalVelocity;

    private float gravityMultiplier = 50f;
    private bool isGrounded;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerInputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        playerInputAction.Player.Jump.started += OnJump;
        move = playerInputAction.Player.Move;
        playerInputAction.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Player.Jump.started -= OnJump;
        playerInputAction.Player.Disable();
    }

    void Start()
    {
        moveDirection = Vector2.zero;
    }

    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, avatarHeight, 0), 0.4f, groundLayer) || Physics.CheckSphere(transform.position - new Vector3(0, avatarHeight, 0), 0.4f, obstacleLayer);

        forceDirection = forceDirection + moveDirection.x * GetCameraRight(playerCamera) * moveSpeed;
        forceDirection = forceDirection + moveDirection.y * GetCameraForward(playerCamera) * moveSpeed;

        playerRigidbody.AddForce(forceDirection, ForceMode.Impulse);

        forceDirection = Vector3.zero;

        // Incrementing the down force when the player is falling down
        if (playerRigidbody.velocity.y < 0f)
            playerRigidbody.velocity = playerRigidbody.velocity + Vector3.down * gravityMultiplier * Time.deltaTime;

        // Limiting the speed of the player
        horizontalVelocity = playerRigidbody.velocity;
        horizontalVelocity.y = 0;
        
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            playerRigidbody.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * playerRigidbody.velocity.y;

        //ControlRotation();
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        if (isGrounded)
            forceDirection = forceDirection + Vector3.up * jumpForce;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;

        return right.normalized;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;

        return forward.normalized;
    }
}
