using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudio : MonoBehaviour
{
    private const float MAXIMUM_SPEED = 10.0f;

    [SerializeField] private float minPitch = 0.1f;
    [SerializeField] private float maxPitch = 2.0f;

    private CarController carController;
    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        carController = GetComponent<CarController>();
        audio = GetComponent<AudioSource>();
        audio.pitch = minPitch;
    }

    // Update is called once per frame
    void Update()
    {
        audio.pitch = Mathf.Clamp(Mathf.Abs(carController.currentVelocity) / MAXIMUM_SPEED, minPitch, maxPitch);
    }
}
