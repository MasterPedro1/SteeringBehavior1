using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorController : MonoBehaviour
{
    public List<Steering> behavior;
    public float maxSpeed = 5f;
    public float maxForce = 5f;
    
    private Vector3 _totalForce;
    private Vector3 _velocity;

    private void FixedUpdate()
    {
        _totalForce = Vector3.zero;
        foreach (var behavior in behavior) 
        {
            behavior.Velocity = _velocity;
            behavior.Position = transform.position;
            _totalForce += LimitVector(behavior.GetForce(), maxForce);
        }

        _velocity = LimitVector(_velocity +  _totalForce, maxSpeed);
        transform.position += _velocity * Time.deltaTime;

    }

    private Vector3 LimitVector(Vector3 force, float limit) 
    {
        return Vector3.ClampMagnitude(force, limit);
    }
}
