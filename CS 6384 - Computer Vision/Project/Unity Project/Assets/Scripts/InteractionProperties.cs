using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionProperties : MonoBehaviour
{
    public enum Interaction { None, Move, Rotate, Scale };
    public Interaction interaction;

    public Vector3 axes = Vector3.zero;
}