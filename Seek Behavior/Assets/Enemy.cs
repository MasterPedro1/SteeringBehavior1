using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.ConstrainedExecution;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Transform target;
    [SerializeField] private Transform sprite;
    [SerializeField] private float maxspeed;
    [SerializeField] private float speed;
    [SerializeField] private float cirdist;
    [SerializeField] private float time = 5;
    [SerializeField] private float time2 = 1;
    [SerializeField] private float angle;
    [SerializeField] private float T = 2;

    public PLayer player;

    float circleradius;
    private Vector3 targetWander;

    private Vector3 steering;

    private Queue<Vector3> targetsQueue = new Queue<Vector3>();



    private enum SteeringType
    {
        Seek, RunAway, Arrival, Wander, Pursuit
    }

    [SerializeField] private SteeringType type = SteeringType.Seek;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    [SerializeField] private float slowingRadios;

    //Son delegados que no aceptan ningun parametro uelven ningun valor


    void Start()
    {
        targetWander = transform.position;

        actions.Add("Seek", CalculateSeek);
        actions.Add("RunAway", CalculateRunAway);
        actions.Add("Arrival", CalculateArrival);
        actions.Add("Wander", CalculateWander);
        actions.Add("Pursuit", CalculatePursuit);
        FillQueue();

        

        StartCoroutine(CountdownCoroutine());

    }

    void Update()
    {
        if (actions.TryGetValue(type.ToString(), out Action action))
        {
            action();
        }

       
    }

    void Move(Vector3 steering)
    {
        velocity += steering;
        velocity = Vector3.ClampMagnitude(velocity, maxspeed);
        transform.position += velocity * Time.deltaTime;
    }

    void CalculateSeek()
    {
        steering = Seek(target.position);

        Move(steering);
    }

    Vector3 Seek(Vector3 target)
    {
        Vector3 desvel = (target - transform.position).normalized * maxspeed;

        return desvel - velocity;
    }

    void CalculateRunAway()
    {
        steering = Seek(target.position);

        Move(steering * -1);
        
    }

    void CalculateArrival()
    {

        Vector3 desvel = (target.position - transform.position);
        float distance = desvel.magnitude;

        if (distance < slowingRadios)
        {
            steering = desvel - velocity * (distance / slowingRadios);
        }
        else
        {
            steering = desvel - velocity;
        }
        steering = desvel - velocity;
        velocity += steering;
        transform.position += velocity * Time.deltaTime;
    }

    void CalculateWander()
    {
        Vector3 target2 = targetWander;
        steering = Seek(target2);
        Move(steering);

        Vector3 circlecent = velocity.normalized * cirdist;

        //Quaternion representa la rotacion de un angulo
        
        Vector3 displacement = Vector3.forward * circleradius;
        Quaternion rotate = Quaternion.AngleAxis(angle, displacement);
        displacement= rotate * displacement;

        Vector3 wanderforce = circlecent + displacement;

        Vector3 circlepos = transform.position + circlecent;


        Debug.DrawLine(transform.position, transform.position + circlecent,Color.red);
        Debug.DrawLine(circlepos, circlepos + displacement, Color.blue);
    }
        
    void CalculatePursuit()
    {
        Vector3 puruit = player.transform.position + (player.m_prevPosition * T);

        steering = Seek(puruit);
        Move(steering);
    }



    private void FillQueue()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomTarget = new Vector3(UnityEngine.Random.Range(-8f, 8f), (UnityEngine.Random.Range(-5f, 5f)), 0);
            targetsQueue.Enqueue(randomTarget);
            //Sprite.transform.position = randomTarget;
        }
    }

    IEnumerator CountdownCoroutine()
    {

        while (true)
        {

            yield return new WaitForSeconds(time);


            if (targetsQueue.Count == 0) FillQueue();

            targetWander = targetsQueue.Dequeue();
            sprite.position = targetWander;

        }

    }

    IEnumerator ChangeAngle()
    {
        yield return new WaitForSeconds(time2);
        float angle = UnityEngine.Random.Range(100f, 50f);

    }
}
