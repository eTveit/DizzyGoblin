using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
Simple steering behaiors for path following / goal seeking and obstacle avoidance

for the cube objects to be static obstacles in the path, disable the steering script.
for the cube objects to behave like the sphere vehicle, enable the script.

you could also create a boolean property that could be set in the inspector.

*/
public class steering : MonoBehaviour {

    public Vector3 velocity = new Vector3(0, 0, 1.5f);
    private Vector3 goalVelocity = new Vector3(0, 0, 1.5f); //the velocity we want to smooth too
    public float maxSpeed = 5.0f;
    public float speed = 0;
    public float radius = 2.0f;

    //STEERING
    public bool useKeyboard = false;
    public Vector3 steeringForce = new Vector3(0, 0, 0);
    public float steeringForceFactor = 0.01f;
    public float dotproduct = 0;

    //PATH
    public Transform path = null;
    public Transform obstacles = null;

    public int curPoint = 0;
    public int pointCount = 0;
    private Vector3[] points;

    public Transform Player;
    public Vector3 goal;


    public enum STATES
    {

        WAIT = 0,
        SEEK = 1,
        FLEE = 2,
        HIT = 3,
    }
     
    public STATES state = STATES.SEEK;

    // Use this for initialization
    void Start()
    {

        if (path)
        {

            foreach (Transform pathpoint in path)
            {
                Debug.Log(pathpoint.name);
                pointCount++;
            }

            points = new Vector3[pointCount];

            int i = 0;
            foreach (Transform pathpoint in path)
            {
                points[i] = pathpoint.position;
                i++;
            }
        }


    }

	// Update is called once per frame
	void Update () {

        float dt = Time.deltaTime;
        
        handlePath(dt);

        handleObstacles(dt);

        if (state == STATES.SEEK)
            seek(dt);
        else if (state == STATES.FLEE)
            flee(dt);
        else if (state == STATES.WAIT)
            wait(dt);

        handleMove(dt);

    }

    void handlePath(float dt)
    {
        Vector3 target = points[curPoint];

        if (Vector3.Distance(target, transform.position) < 2.0f)
        {
            
             
            //to follow a path in sequence
            curPoint++;

            if (curPoint >= pointCount)
                curPoint = 0;

                       
                
            /*
            //to move to random points
            curPoint = Random.Range(0, pointCount - 1);

            target = points[curPoint];
            */

        }
        
        Vector3 targetDirection = Vector3.Normalize(target - transform.position);

        velocity += targetDirection * dt * 50;

        /*
        dotproduct = Vector3.Dot(targetDirection, transform.forward);

        if (dotproduct > 0 &&  Mathf.Abs(dotproduct) > 0.1f )
            steeringForce += transform.right * dt * 10;
        else if (dotproduct < 0 && Mathf.Abs(dotproduct) > 0.1f)
            steeringForce -= transform.right * dt * 10;
        else
        {
            //Debug.Log("steeringForce nuked!!");
            //steeringForce = new Vector3(0, 0, 0);
        }
        */



    }

    void handleObstacles(float dt)
    {


        if (obstacles == null)
            return;

        foreach (Transform obstacle in obstacles)
        {


            float dist = Vector3.Distance(transform.position, obstacle.position);
            if (dist < 4.0f && obstacle.transform.gameObject != this.transform.gameObject)
            {
                Vector3 targetDirection = Vector3.Normalize(transform.position - obstacle.position);
                
                if (dist <= 0.0001f)
                    dist = 0.0001f;

                float distfactor = 4.0f / dist;

                targetDirection = Vector3.Slerp(targetDirection, transform.forward, (distfactor * dt));
                
                velocity += targetDirection * dt * (30 * distfactor);

            }

            

        }


    }

    void seek(float dt)
    {

        Vector3 target = Player.position;

        if (Vector3.Distance(target, transform.position) < 0.5f)
        {

            state = STATES.HIT;
            return;
        }
        
        Vector3 targetDirection = Vector3.Normalize(target - transform.position);

        velocity += targetDirection * dt * 50;

    }

    void flee(float dt)
    {
        Vector3 target = Player.position;

        if (Vector3.Distance(target, transform.position) > 20.0f)
        {

            state = STATES.SEEK;
            return;
        }

        Vector3 targetDirection = Vector3.Normalize(transform.position - target);

        velocity += targetDirection * dt * 50;

    }
    void wait(float dt)
    {

        int x = Random.Range((int)transform.position.x - 2, (int)transform.position.x + 2);
        int z = Random.Range((int)transform.position.z - 2, (int)transform.position.z + 2);

        Vector3 target = new Vector3(x, 0, z);


        if (Vector3.Distance(target, transform.position) < 0.5f)
        {

            //find a new point

        }

        Vector3 targetDirection = Vector3.Normalize(target - transform.position);

        velocity += targetDirection * dt * 50;


    }


    void handleMove(float dt)
    {


              
        //velocity += (steeringForce * steeringForceFactor);

        //GENERAL RULE OF VELOCITY : don't let them go too fast!!!        
        float maxSpeedSquared = maxSpeed * maxSpeed;
        float velMagSquared = velocity.magnitude * velocity.magnitude;
        if (velMagSquared > maxSpeedSquared)
        {
            velocity *= (maxSpeed / velocity.magnitude);
        }
                
        //and then we "normalize" to get a heading, or rather, a lookAt position
        Vector3 heading = velocity;
        heading.Normalize();
        Vector3 lookAtPoint = transform.position + (heading * 2); //look a bit in front of me
        transform.LookAt(lookAtPoint);    //we may want to interpolate the turn (or the camera)


        transform.position += velocity * dt;

    }
}
