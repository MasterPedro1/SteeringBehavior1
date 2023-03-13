using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : Steering
{
    public bool arrival = false;
    public float slowingRadius = 5f;

    public override Vector3 GetForce()
    {
        Desiredvelocity = (Target - Position).normalized * speed;
        return Desiredvelocity - Velocity;
    }
}
