using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDigits : MonoBehaviour
{
    private Landmarks landmarks;
    public GameObject thumb;
    public GameObject index;
    public GameObject middle;
    public GameObject ring;
    public GameObject little;

    // Start is called before the first frame update
    void Start()
    {
        landmarks = FindObjectOfType<Landmarks>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 thumbPosition = landmarks.leftThumbTipPosition;
        //thumbPosition.y = -thumbPosition.y;
        //thumbPosition.z = transform.position.z;

        //Vector3 indexPosition = landmarks.leftIndexTipPosition;
        //indexPosition.y = -indexPosition.y;
        //indexPosition.z = transform.position.z;

        //Vector3 middlePosition = landmarks.leftMiddleTipPosition;
        //middlePosition.y = -middlePosition.y;
        //middlePosition.z = transform.position.z;

        //Vector3 ringPosition = landmarks.leftRingTipPosition;
        //ringPosition.y = -ringPosition.y;
        //ringPosition.z = transform.position.z;

        //Vector3 littlePosition = landmarks.leftLittleTipPosition;
        //littlePosition.y = -littlePosition.y;
        //littlePosition.z = transform.position.z;

        //thumb.transform.position = thumbPosition;
        //index.transform.position = indexPosition;
        //middle.transform.position = middlePosition;
        //ring.transform.position = ringPosition;
        //little.transform.position = littlePosition;
    }
}
