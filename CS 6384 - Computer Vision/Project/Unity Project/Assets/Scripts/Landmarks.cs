using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmarks : MonoBehaviour
{
    public Vector3 leftOriginPosition { get; private set; }
    public Vector3 leftThumbTipPosition { get; private set; }
    public Vector3 leftIndexTipPosition { get; private set; }

    public Vector3 leftMiddleBasePosition { get; private set; }

    public Vector3 leftOriginPositionNormalized { get; private set; }
    public Vector3 leftMiddleBasePositionNormalized { get; private set; }

    public Vector3 rightOriginPosition { get; private set; }
    public Vector3 rightThumbTipPosition { get; private set; }
    public Vector3 rightIndexTipPosition { get; private set; }

    public Vector3 rightMiddleBasePosition { get; private set; }

    public Vector3 rightOriginPositionNormalized { get; private set; }
    public Vector3 rightMiddleBasePositionNormalized { get; private set; }

    private Camera cam;
    private Client client;

    private float screenWidth;
    private float screenHeight;
    private float depth;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        client = FindObjectOfType<Client>();
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        depth = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        SetLeftOriginLandmarks();
        SetLeftBaseLandmarks();

        SetRightOriginLandmarks();
        SetRightBaseLandmarks();
    }

    private void SetLeftOriginLandmarks()
    {
        leftOriginPositionNormalized = client.leftOriginPosition;
        float originX = screenWidth * leftOriginPositionNormalized.x;
        float originY = screenHeight * leftOriginPositionNormalized.y;
        float originZ = screenWidth * leftOriginPositionNormalized.z;
        Vector3 originPositionLocal = cam.ScreenToWorldPoint(new Vector3(originX, originY, depth));
        leftOriginPosition = originPositionLocal;
    }

    private void SetLeftBaseLandmarks()
    {
        leftMiddleBasePositionNormalized = client.leftMiddleBasePosition;
        float middleBaseX = screenWidth * leftMiddleBasePositionNormalized.x;
        float middleBaseY = screenHeight * leftMiddleBasePositionNormalized.y;
        float middleBaseZ = screenWidth * leftMiddleBasePositionNormalized.z;

        Vector3 middleBasePositionLocal = cam.ScreenToWorldPoint(new Vector3(middleBaseX, middleBaseY, depth));


        leftMiddleBasePosition = new Vector3(middleBasePositionLocal.x, middleBasePositionLocal.y, middleBasePositionLocal.z + middleBaseZ);
    }

    private void SetRightOriginLandmarks()
    {
        rightOriginPositionNormalized = client.rightOriginPosition;
        float originX = screenWidth * rightOriginPositionNormalized.x;
        float originY = screenHeight * rightOriginPositionNormalized.y;
        float originZ = screenWidth * rightOriginPositionNormalized.z;
        Vector3 originPositionLocal = cam.ScreenToWorldPoint(new Vector3(originX, originY, depth));
        rightOriginPosition = originPositionLocal;
    }

    private void SetRightBaseLandmarks()
    {
        rightMiddleBasePositionNormalized = client.rightMiddleBasePosition;
        float middleBaseX = screenWidth * rightMiddleBasePositionNormalized.x;
        float middleBaseY = screenHeight * rightMiddleBasePositionNormalized.y;
        float middleBaseZ = screenWidth * rightMiddleBasePositionNormalized.z;

        Vector3 middleBasePositionLocal = cam.ScreenToWorldPoint(new Vector3(middleBaseX, middleBaseY, depth));

        rightMiddleBasePosition = new Vector3(middleBasePositionLocal.x, middleBasePositionLocal.y, middleBasePositionLocal.z + middleBaseZ);
    }
}