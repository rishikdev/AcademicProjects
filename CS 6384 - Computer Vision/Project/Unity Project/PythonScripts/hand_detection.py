import json
import cv2
import mediapipe as mp
import zmq
import numpy as np

mp_drawing = mp.solutions.drawing_utils
mp_drawing_styles = mp.solutions.drawing_styles
mp_hands = mp.solutions.hands

context = zmq.Context()
socket = context.socket(zmq.PUB)
socket.bind("tcp://*:5556")
topic = "mp"

def distance(point1, point2):
    return np.linalg.norm(point1 - point2)

def detectPose(landmark):
    pose = 'none'

    originLandmark = np.array([landmark[0][0].x, landmark[0][0].y])
    thumbTipLandmark = np.array([landmark[0][4].x, landmark[0][4].y])
    indexTipLandmark = np.array([landmark[0][8].x, landmark[0][8].y])
    middleTipLandmark = np.array([landmark[0][12].x, landmark[0][12].y])
    ringTipLandmark = np.array([landmark[0][16].x, landmark[0][16].y])
    littleTipLandmark = np.array([landmark[0][20].x, landmark[0][20].y])

    fistThreshold = 0.2
    pointThreshold = 0.05

    indexToOrigin = distance(indexTipLandmark, originLandmark)
    littleToOrigin = distance(littleTipLandmark, originLandmark)
    indexToThumb = distance(indexTipLandmark, thumbTipLandmark)

    if indexToOrigin <= fistThreshold and littleToOrigin <= fistThreshold:
        pose = 'fist'

    elif indexToOrigin > fistThreshold and littleToOrigin > fistThreshold and indexToThumb > pointThreshold:
        pose = 'stop'

    elif indexToOrigin > fistThreshold and littleToOrigin <= fistThreshold and indexToThumb > pointThreshold:
        pose = 'point'

    elif indexToThumb <= pointThreshold:
        pose = 'OK'

    return pose

def sendData(hand, landmark):
    originLandmark = landmark[0][0]
    thumbTipLandmark = landmark[0][4]
    indexTipLandmark = landmark[0][8]
    middleTipLandmark = landmark[0][12]
    ringTipLandmark = landmark[0][16]
    littleTipLandmark = landmark[0][20]

    indexBaseLandmark = landmark[0][5]
    middleBaseLandmark = landmark[0][9]
    ringBaseLandmark = landmark[0][13]
    littleBaseLandmark = landmark[0][17]

    pose = detectPose(landmark=landmark)

    landmarkDict = {
                    "topic" : topic,
                    "hand" : hand,
                    "pose" : pose,

                    "originLandmark" : [originLandmark.x, originLandmark.y, originLandmark.z],
                    
                    "thumbTipLandmark" : [thumbTipLandmark.x, thumbTipLandmark.y, thumbTipLandmark.z],
                    "indexTipLandmark" : [indexTipLandmark.x, indexTipLandmark.y, indexTipLandmark.z],
                    "middleTipLandmark" : [middleTipLandmark.x, middleTipLandmark.y, middleTipLandmark.z],
                    "ringTipLandmark" : [ringTipLandmark.x, ringTipLandmark.y, ringTipLandmark.z],
                    "littleTipLandmark" : [littleTipLandmark.x, littleTipLandmark.y, littleTipLandmark.z],

                    "indexBaseLandmark" : [indexBaseLandmark.x, indexBaseLandmark.y, indexBaseLandmark.z],
                    "middleBaseLandmark" : [middleBaseLandmark.x, middleBaseLandmark.y, middleBaseLandmark.z],
                    "ringBaseLandmark" : [ringBaseLandmark.x, ringBaseLandmark.y, ringBaseLandmark.z],
                    "littleBaseLandmark" : [littleBaseLandmark.x, littleBaseLandmark.y, littleBaseLandmark.z]
                }
    
    socket.send_string("%s %s" % (topic, json.dumps(landmarkDict)))

def main(showImage):
    cap = cv2.VideoCapture(0)
    with mp_hands.Hands(
        model_complexity=0,
        min_detection_confidence=0.5,
        min_tracking_confidence=0.5,
        max_num_hands=2) as hands:

        while cap.isOpened():
            success, image = cap.read()
            image = cv2.flip(image, 1)
            if not success:
                print("Ignoring empty camera frame.")
                # If loading a video, use 'break' instead of 'continue'.
                continue

            # To improve performance, optionally mark the image as not writeable to
            # pass by reference.
            image.flags.writeable = False
            image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
            results = hands.process(image)

            if showImage:
                # Draw the hand annotations on the image.
                image.flags.writeable = True
                image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)

            if results.multi_hand_landmarks:
                leftLandmark = []
                rightLandmark = []

                for i, hand_landmarks in enumerate(results.multi_hand_landmarks):
                    if showImage:
                        mp_drawing.draw_landmarks(image, hand_landmarks, mp_hands.HAND_CONNECTIONS, mp_drawing_styles.get_default_hand_landmarks_style(), mp_drawing_styles.get_default_hand_connections_style())
                    
                    hand = results.multi_handedness[i].classification[0].label
                    if hand == "Left":
                        leftLandmark.append(hand_landmarks.landmark)
                        sendData(hand=hand, landmark=leftLandmark)
                    
                    else:
                        rightLandmark.append(hand_landmarks.landmark)
                        sendData(hand=hand, landmark=rightLandmark)

            # Flip the image horizontally for a selfie-view display.
            # cv2.imshow('MediaPipe Hands', cv2.flip(image, 1))
            if showImage:
                cv2.imshow('MediaPipe Hands', image)

            if cv2.waitKey(5) & 0xFF == ord('q'):
                break
        
    cap.release()

if __name__ == "__main__":
    main(False)