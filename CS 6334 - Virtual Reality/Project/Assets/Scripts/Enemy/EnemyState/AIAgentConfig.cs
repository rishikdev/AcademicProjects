﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject
{
    public float maxTime = 0.25f;
    public float minDistance = 1.0f;
    public float dieForce = 10.0f;
    public float maxSightDistance = 10.0f;
}
