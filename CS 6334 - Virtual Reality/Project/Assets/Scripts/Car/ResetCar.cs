using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResetCar : MonoBehaviour
{
    [SerializeField] private PlayerInputAction playerInputAction;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        playerInputAction.Vehicle.Reset.started += Reset;
        playerInputAction.Vehicle.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Vehicle.Reset.started -= Reset;
        playerInputAction.Vehicle.Disable();
    }

    private void Reset(InputAction.CallbackContext obj)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);
    }
}
