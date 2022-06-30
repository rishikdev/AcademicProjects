using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json.Linq;

public class Client : MonoBehaviour
{
    private enum Hand { Left, Right }
    public enum HandPose { OK, Stop, Fist, Point, None }

    private SubscriberSocket client;
    private string topic = "mp";
    private string url = "tcp://127.0.0.1:5556";

    private Hand hand;
    public HandPose leftHandPose;
    public HandPose rightHandPose;

    public Vector3 leftOriginPosition { get; private set; }
    public Vector3 leftThumbTipPosition { get; private set; }
    public Vector3 leftIndexTipPosition { get; private set; }
    public Vector3 leftMiddleBasePosition { get; private set; }

    public Vector3 rightOriginPosition { get; private set; }
    public Vector3 rightThumbTipPosition { get; private set; }
    public Vector3 rightIndexTipPosition { get; private set; }
    public Vector3 rightMiddleBasePosition { get; private set; }

    private void Start()
    {
        client = new SubscriberSocket();
        client.Connect(url);
        client.Subscribe(topic);
        Debug.Log("Connecting");
    }

    private void Update()
    {
        string messageReceived;
        if (!client.TryReceiveFrameString(out messageReceived))
            return;

        messageReceived = messageReceived.Substring(topic.Length + 1);
        JObject jObjectMessage = JObject.Parse(messageReceived);

        SetHand(jObjectMessage);
        if (hand == Hand.Left)
        {
            SetLeftPose(jObjectMessage);
            SetLeftOriginPosition(jObjectMessage);
            SetLeftBasePositions(jObjectMessage);
        }

        else
        {
            SetRightPose(jObjectMessage);
            SetRightOriginPosition(jObjectMessage);
            SetRightBasePositions(jObjectMessage);
        }
    }

    private void SetHand(JObject jObjectMessage)
    {
        hand = (jObjectMessage["hand"].ToObject<string>() == "Right") ? Hand.Right : Hand.Left;
    }

    private void SetLeftPose(JObject jObjectMessage)
    {
        string poseString = jObjectMessage["pose"].ToObject<string>();

        switch(poseString)
        {
            case "OK":
                leftHandPose = HandPose.OK;
                break;

            case "stop":
                leftHandPose = HandPose.Stop;
                break;

            case "fist":
                leftHandPose = HandPose.Fist;
                break;

            case "point":
                leftHandPose = HandPose.Point;
                break;

            default:
                leftHandPose = HandPose.None;
                break;
        }
    }

    private void SetLeftOriginPosition(JObject jObjectMessage)
    {
        JArray originLandmark = (JArray)jObjectMessage["originLandmark"];
        leftOriginPosition = new Vector3(originLandmark[0].ToObject<float>(), originLandmark[1].ToObject<float>(), originLandmark[2].ToObject<float>());
    }

    private void SetLeftBasePositions(JObject jObjectMessage)
    {
        JArray middleBaseLandmark = (JArray)jObjectMessage["middleBaseLandmark"];

        leftMiddleBasePosition = new Vector3(middleBaseLandmark[0].ToObject<float>(), middleBaseLandmark[1].ToObject<float>(), middleBaseLandmark[2].ToObject<float>());
    }

    private void SetRightPose(JObject jObjectMessage)
    {
        string poseString = jObjectMessage["pose"].ToObject<string>();

        switch (poseString)
        {
            case "OK":
                rightHandPose = HandPose.OK;
                break;

            case "stop":
                rightHandPose = HandPose.Stop;
                break;

            case "fist":
                rightHandPose = HandPose.Fist;
                break;

            case "point":
                rightHandPose = HandPose.Point;
                break;

            default:
                rightHandPose = HandPose.None;
                break;
        }
    }

    private void SetRightOriginPosition(JObject jObjectMessage)
    {
        JArray originLandmark = (JArray)jObjectMessage["originLandmark"];
        rightOriginPosition = new Vector3(originLandmark[0].ToObject<float>(), originLandmark[1].ToObject<float>(), originLandmark[2].ToObject<float>());
    }

    private void SetRightBasePositions(JObject jObjectMessage)
    {
        JArray middleBaseLandmark = (JArray)jObjectMessage["middleBaseLandmark"];

        rightMiddleBasePosition = new Vector3(middleBaseLandmark[0].ToObject<float>(), middleBaseLandmark[1].ToObject<float>(), middleBaseLandmark[2].ToObject<float>());
    }
}