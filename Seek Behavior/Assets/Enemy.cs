using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Transform target;
    [SerializeField] private float speed;

    private Vector3 steering;

    private enum SteeringType
    {
        Seek, RunAway, Arrival
    }

    [SerializeField] private SteeringType type = SteeringType.Seek;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    //Son delegados que no aceptan ningun parametro uelven ningun valor

    // Start is called before the first frame update
    void Start()
    {
        actions.Add("Seek", CalculateSeek);
        actions.Add("RunAway",CulateRunAway);
    }

    // Update is called once per frame
    void Update()
    {
        if(actions.TryGetValue(type.ToString(), out Action action))
        {
            action();
        }
    }

    void CalculateSeek()
    {
        Vector3 desvel = (target.position - transform.position).normalized * speed;

        steering = desvel - velocity;

        velocity += steering;
        transform.position += velocity * Time.deltaTime;
    }

    void CulateRunAway()
    {
        Vector3 desvel = (target.position - transform.position).normalized * speed;

        steering = desvel - velocity;

        velocity += steering;
        transform.position -= velocity * Time.deltaTime;
    }
}
