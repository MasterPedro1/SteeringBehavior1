using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Steering : MonoBehaviour
{

    public float speed = 10f;
    public Vector3 Desiredvelocity;
    public Vector3 Velocity;
    public Vector3 Position;
    public Vector3 Target;

    public abstract Vector3 GetForce();

}
