# Interacting with objects in virtual environment using hand pose estimation

This project was done in collaboration with [Ritwik Dev](https://github.com/RitwikDev).

## Overview

| <img width="1277" alt="Overview" src="https://user-images.githubusercontent.com/82426895/166475478-d279e0e4-04aa-4da4-a4ae-fdffb6cae903.png"> |
|:--:|
|*Overview of our project*|

In this project we demonstrate interacting with objects in virtual environment using hand pose estimation and a ***single RGB camera***. We have used ***MediaPipe's hand solution*** and ***OpenCV*** in the backend, ***Unity Game Engine*** in the frontend, and ***ZeroMQ and NetMQ*** to transfer data between the backend and the frontend.

## Backend + ZeroMQ

In the backend, we are using [MediaPipe's hand solution](https://google.github.io/mediapipe/solutions/hands.html) and OpenCV. It tracks 21 3D landmarks on each hands in real time. Even though the model is tracking a total of 42 landmarks, it still achieves an impressive 30+ frames per second on a mobile (laptop) CPU. For recording the hands, we are using the device's built-in camera.

We are aggregating the data and sending the data to the frontend (Unity Game Engine) using ZeroMQ. ZeroMQ is a high performance networking library. It publishes the data to a topic and the frontend get the desired data by subscribing to the same topic.

## Frontend

| <img width="1277" alt="Frontend" src="https://user-images.githubusercontent.com/82426895/166560855-2961ee63-e350-44f5-adc7-623affc66bd8.png"> |
|:--:|
|*Frontend: Unity Game Engine*|

In the frontend, we have a virtual environment built using Unity Game Engine. It contains a capsule, a cube, and a cylinder. Each of these objects is called gameobject in Unity. The virtual environment also contains 3D models of both the left hand as well as the right hand. When the simulation (game) begins, the camera turns on and starts detecting the user's hands (if visible). The user can move, rotate, and scale the gameobjects. Some properties such as gravity, and colour of the gameobject can also be changed. 

We have defined some gestures to interact with the gameobjects and also to bring up the menu and the context menu.

| <img width=700 alt="Gestures" src="https://user-images.githubusercontent.com/82426895/166563985-45b2ebcd-ff14-45c8-b688-6f541592bd9c.png"/> |
|:--:|
|*Gestures: Stop (top left), Fist (top right), Pinch/Pick (bottom left), Point (bottom right)*|

- To *bring up the menu* that lists the actions that can be performed on the game objects (move, rotate, and scale), with the left hand (this gesture is defined on the left hand only) *perform the following sequence of gestures: fist, stop, fist, and stop*. 
- To *select* any action, use your index finger (pointing gesture is not necessary). 
- To *interact* with the gameobjects (move, rotate, and scale), use the pinching/picking gesture.
- To *stop interacting* with the gameobject (for example, after moving the gameobject, you may want to place it somewhere in the scene), use the *stop gesture*.

- To *bring up the context menu*, touch the gameobject whose properties you want to change with the *pointing gesture* for a couple of seconds.

To see how the actions are performed, you can watch [this](https://youtu.be/1cz3UxKJ6c8) video.

#### About Gestures

| <img alt="landmarks" src="https://google.github.io/mediapipe/images/mobile/hand_landmarks.png"/> |
|:--:|
|*Landmarks*|

We tried classifying the gestures using [tensforflow](https://techvidvan.com/tutorials/hand-gesture-recognition-tensorflow-opencv/). But, this slowed down our project to the point where it was barely usable. So, we decided to classify the gestures using the landmarks. We measured the distances between different landmarks to classify between different gestures. 

- **Fist** - The distance between the tip of the in- dex finger (landmark 8) and the wrist (landmark 0) should be less than fist threshold. Moreover, the distance between the tip of the pinky (landmark 20)and the wrist should be less than the aforementioned threshold.

- **Stop** - The distance between the tip of the index finger and the wrist should be greater than the fist threshold. Likewise, the distance between the tip of the pinky and the wrist should be greater than the fist threshold. Furthermore, the distance between the tip of the index finger and the tip of the thumb (landmark 4) should be greater than point threshold.

- **Point** - The distance between the tip of the index finger and the wrist should be greater than the fist threshold. Moreover, the distance between the tip of the pinky and the wrist should be less than the fist threshold. Furthermore, the distance between the tip of the index finger and the tip of the thumb (landmark 4) should be greater than point threshold.

- **Pinch** - The distance between the tip of the index finger and the tip of the thumb (landmark 4) should be less than point threshold.

#### About Depth

To calculate depth, we are calculating the distance between the landmarks 9 and 0. This is not perfect, however, as, if this distance changes for any reason, we cannot, at this point, confirm if the change was indeed due to the hand travelling along the depth axis. Since we are using just a single RGB camera, we do not have any depth information, so this is the only way we could implement this feature.

## Requirements

To test this project on your computer, you need to have the following softwares installed:

- [Python](https://www.python.org/downloads/) (we tested this project on Python 3.8.x and 3.9.x)
- [MediaPipe*](https://google.github.io/mediapipe/getting_started/install.html)
- [OpenCV](https://pypi.org/project/opencv-python/)
- [ZeroMQ](https://zeromq.org/download/)
- [Unity Game Engine](https://unity.com/download) (if you want to build this project for your operating system)

We used macOS 12.3 and macOS 11.6 to develop this project. We have also provided the latest build (<code>.app</code> file) if you have a Mac to try it on.

##### *Quick Note!
I used MacBook Air (M1, 2020). Follow [this link](https://stackoverflow.com/a/69384092) if you want to install MediaPipe on your ARM powered Mac.

## How to run this project

To run this project, first run the <code>hand_detection.py</code> file located in **Python Scripts**. This should turn your device's default camera on. Then, run the application (if you do not have the application, you will have to build it first using Unity). In this way, you should be able to run the project. After you have used the application, make sure to **stop** the <code>hand_detection.py</code> program.
