using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.ConstrainedExecution;

public class SteeringBehavior_MCP : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Transform target;
    [SerializeField] private SteeringType type;
    [SerializeField] private float timeW = 3;
    [SerializeField] private float timeA = 3;
    private Vector3 velocity;
    private Queue<Vector3> targetsQueue = new Queue<Vector3>();
    private Vector3 steering;


    [Header("Wander")]
    [SerializeField] private float circledistance;
    [SerializeField] private float circleradius;
    [SerializeField] private Vector3 targetChange;
    [SerializeField] private float anglec = 0;
    //[SerializeField] private Transform sp;
    private Vector3 targetwander;
    private Vector3 wanderforce;



    [Header("Seek & Run Away")]
    [SerializeField] private float speed = 2;
    [SerializeField] private float maxspeed = 2;
    public Vector3 seekTarget;





    private enum SteeringType
    {
        Wander, Seek, RunAway
    }


    private Dictionary<string, Action> actions = new Dictionary<string, Action>();


    void Start()
    {
        actions.Add("Seek", CalculateSeek);
        actions.Add("RunAway", CalculateRunAway);
        actions.Add("Wander", CalculateWander);
        FillQueue();
        StartCoroutine(CountdownCoroutine());
        StartCoroutine(Angle());
    }


    // Update is called once per frame
    void Update()
    {
        if (actions.TryGetValue(type.ToString(), out Action action))
        {
            action();
        }
    }


    void CalculateSeek()
    {
        seekTarget = target.position;
        steering = Seek(seekTarget);

        Move(steering);
    }


    Vector3 Seek(Vector3 target)
    {
        Vector3 desvel = (target - transform.position).normalized * speed;

        return desvel - velocity;
    }


    void Move(Vector3 steering)
    {
        velocity += steering;
        velocity = Vector3.ClampMagnitude(velocity, maxspeed);
        transform.position += velocity * Time.deltaTime;
    }


    void CalculateRunAway()
    {
        seekTarget = target.position;
        steering = Seek(seekTarget);

        Move(steering * -1);
    }


    void CalculateWander()
    {
        seekTarget = targetwander;

        Vector3 circenter = Vector3.Normalize(velocity) * circledistance;
        Vector3 circlepos = transform.position + circenter;
        Debug.DrawLine(transform.position, circlepos, Color.green);


        Vector3 displacement = Vector3.up * circleradius;
        Quaternion rotate = Quaternion.AngleAxis(anglec, Vector3.forward);
        displacement = rotate * displacement;
        Debug.DrawLine(circlepos, circlepos + displacement, Color.blue);

        Vector3 wanderforce = circenter + displacement;
        Debug.DrawLine(transform.position, transform.position + wanderforce, Color.red);



       
        steering = Seek(wanderforce) + Seek(seekTarget);
        Move(steering);

    }

    private void FillQueue()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomTarget = new Vector3(UnityEngine.Random.Range(-8f, 8f), (UnityEngine.Random.Range(-5f, 5f)), 0);
            targetsQueue.Enqueue(randomTarget);

        }
    }

    IEnumerator CountdownCoroutine()
    {

        while (true)
        {



            if (targetsQueue.Count == 0) FillQueue();

            targetwander = targetsQueue.Dequeue();
            //sp.position = targetwander;

            yield return new WaitForSeconds(timeW);
        }

    }

    IEnumerator Angle()
    {
        while (true)
        {
            float angle = UnityEngine.Random.Range(-50, 50);
            anglec = angle;
            yield return new WaitForSeconds(timeA);
        }
    }


}
