using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Segment
{
    public GameObject prefab;       // The prefab for this segment
    [NonSerialized] public Transform entryPoint;    // The entry point of the segment
    [NonSerialized] public Transform exitPoint;     // The exit point of the segment

    public Segment(GameObject prefab, Transform entryPoint, Transform exitPoint)
    {
        this.prefab = prefab;
        this.entryPoint = entryPoint;
        this.exitPoint = exitPoint;
    }
}