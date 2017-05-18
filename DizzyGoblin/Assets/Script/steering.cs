using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//@KITTIKORN -  I'm implementing the most ultra-optimized collision avoidance system known to man. I invented it,
//              I think. Though probably it already did exist in some other form at some time, perhaps I only
//              rediscovered it. It uses the level manager "occupied" array, also used to place rocks and trees,
//              to simply find the things in my immediate area that I need to avoid.
//              This way, I can do my avoidance without checking each and every possible obstacle in the world. 
//              Rats must then occupy and un-occupy "tiles" in the terrain occupied array as they enter and exit them. 
//              Rocks and trees are already there. I have used this before to great effect.


/* 
Simple steering behaviors for path following / goal seeking and obstacle avoidance

for the cube objects to be static ratObstacles in the path, disable the steering script.
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
    public Transform ratObstacles = null;
	public Transform treeObstacles = null;

    //occupancy data x,z into level manager occupied array - initialize to -1 so I can flag them as "not yet set"
    int lastX = -1;
    int lastZ = -1;


	public int curPoint = 0;
    public int pointCount = 0;
    private Vector3[] points;

    public Transform ratHole;
    public Transform Player;
    public Vector3 goal;
    public TerrainMesh terrain;
    public LJB_levelManager levelManager;

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

        //<JK> optimized!! check it out my young paduans!
        //avoidRats(dt);
        //avoidTrees(dt);
        //avoidObstacles(dt);


		if (state == STATES.SEEK)
			seek(dt);
		else if (state == STATES.FLEE)
			flee(dt);
		else if (state == STATES.WAIT)
			wait(dt);
		else if (state == STATES.HIT)
			hit(dt);

        handleMove(dt);

    }

    void handlePath(float dt)
    {


        if (path == null)
            return;

        if (points.Length < 1)
            return;

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

    //Deprecated!!
    void avoidRats(float dt)
    {


        if (ratObstacles == null)
            return;

        foreach (Transform obstacle in ratObstacles)
        {


            float dist = Vector3.Distance(transform.position, obstacle.position);
            if (dist < 2.0f && obstacle.transform.gameObject != this.transform.gameObject)
            {
                Vector3 targetDirection = Vector3.Normalize(transform.position - obstacle.position);
                
                if (dist <= 0.0001f)
                    dist = 0.0001f;

                float distfactor = 2.0f / dist;

                targetDirection = Vector3.Slerp(targetDirection, transform.forward, (distfactor * dt));
                
                velocity += targetDirection * dt * (30 * distfactor);

            }

            

        }


    }
    void avoidTrees(float dt)
	{

		return;

		if (treeObstacles == null)
			return;

		foreach (Transform obstacle in ratObstacles)
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


    void avoidObstacles(float dt)
    {

        //Unity wastefull! I just want to ignore Y, and I have to allocate a new vector? totally crap!!!
        Vector3 pos = new Vector3 (transform.position.x, 0, transform.position.z);

        int x = Mathf.RoundToInt(pos.x);
        int z = Mathf.RoundToInt(pos.z);
        
        //check the occupied value for the set of xz terrain points closest to me
        //these are the 8 combinations of +- to my current x,z

        //@KITIKORN - can you make a loop such that we don't have to copy/paste so damn many times?
        //            if so, can you expand the area (x+-,z+-) that we check for occupancy, using that loop?
        //@ULTRA-CHALLENGE!! can anyone figure out how to ignore obstacles BEHIND us? answer is in the dot product.

        float tweaker = 50.0f; //adjustable value of "how much" to avoid
        if (levelManager.occupied[terrain.getVertexIndexFromXZ(x + 1, z)] > 0)
        {
            Vector3 avoidPos = new Vector3(x + 1, 0, z);
            Vector3 targetDirection = Vector3.Normalize(pos - avoidPos);
            velocity += targetDirection * dt * tweaker;  

        }
        if (levelManager.occupied[terrain.getVertexIndexFromXZ(x - 1, z)] > 0)
        {
            Vector3 avoidPos = new Vector3(x - 1, 0, z);
            Vector3 targetDirection = Vector3.Normalize(pos - avoidPos);
            velocity += targetDirection * dt * tweaker;
        }
        if (levelManager.occupied[terrain.getVertexIndexFromXZ(x + 1, z+1)] > 0)
        {

            Vector3 avoidPos = new Vector3(x + 1, 0, z + 1);
            Vector3 targetDirection = Vector3.Normalize(pos - avoidPos);
            velocity += targetDirection * dt * tweaker;
        }
        if (levelManager.occupied[terrain.getVertexIndexFromXZ(x - 1, z+1)] > 0)
        {
            Vector3 avoidPos = new Vector3(x - 1, 0, z + 1);
            Vector3 targetDirection = Vector3.Normalize(pos - avoidPos);
            velocity += targetDirection * dt * tweaker;
        }
        if (levelManager.occupied[terrain.getVertexIndexFromXZ(x + 1, z - 1)] > 0)
        {
            Vector3 avoidPos = new Vector3(x + 1, 0, z - 1);
            Vector3 targetDirection = Vector3.Normalize(pos - avoidPos);
            velocity += targetDirection * dt * tweaker;
        }
        if (levelManager.occupied[terrain.getVertexIndexFromXZ(x - 1, z - 1)] > 0)
        {
            Vector3 avoidPos = new Vector3(x - 1, 0, z - 1);
            Vector3 targetDirection = Vector3.Normalize(pos - avoidPos);
            velocity += targetDirection * dt * tweaker;
        }
        if (levelManager.occupied[terrain.getVertexIndexFromXZ(x, z - 1)] > 0)
        {
            Vector3 avoidPos = new Vector3(x , 0, z - 1);
            Vector3 targetDirection = Vector3.Normalize(pos - avoidPos);
            velocity += targetDirection * dt * tweaker;

        }
        if (levelManager.occupied[terrain.getVertexIndexFromXZ(x, z + 1)] > 0)
        {
            Vector3 avoidPos = new Vector3(x , 0, z + 1);
            Vector3 targetDirection = Vector3.Normalize(pos - avoidPos);
            velocity += targetDirection * dt * tweaker;

        }



    }

    void seek(float dt)
    {

        Vector3 target = Player.position;

        if (Vector3.Distance(target, transform.position) < 1.0f)
        {

            state = STATES.HIT;
            return;
        }
        
        Vector3 targetDirection = Vector3.Normalize(target - transform.position);

        velocity += targetDirection * dt * 50;

    }
	void hit(float dt)
	{
		velocity = new Vector3(0, 0, 0);
        Player.GetComponent<HealthSystem>().DamagePlayer(1);
        state = STATES.FLEE;
    }
    void flee(float dt)
    {
        Vector3 target = ratHole.position;

        if (Vector3.Distance(target, transform.position) < 20.0f)
        {

            state = STATES.SEEK;
            return;
        }

        Vector3 targetDirection = Vector3.Normalize(target - transform.position);

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


        Vector3 curpos = transform.position;

        //setting "last values" if not yet set (first frame) for collision avoidance map
        if (lastX < 0)
            lastX = Mathf.RoundToInt(curpos.x);
        if (lastZ < 0)
            lastZ = Mathf.RoundToInt(curpos.z);

        //not yet implemented
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

        float y = terrain.getHeightAt(transform.position);
        Vector3 pos = new Vector3(transform.position.x, y, transform.position.z);

        transform.position = Vector3.Lerp(curpos, pos, dt * speed);

        //occupy a new tile if needed 
        int x = Mathf.RoundToInt(transform.position.x);
        int z = Mathf.RoundToInt(transform.position.z);

        //any time x OR z changes, unoccupy my previous x,z - be sure lastXZ has been set once
        if ((x != lastX && lastX > 0) || (z != lastZ && lastZ > 0))
        {
            //get the index of where i was before I moved into a tile
            //and decrement the occupancy count
            int vi = terrain.getVertexIndexFromXZ(lastX, lastZ);
            levelManager.occupied[vi]--;

            //last is now current
            lastX = x;
            lastZ = z;
            //occupy it
            vi = terrain.getVertexIndexFromXZ(lastX, lastZ);
            levelManager.occupied[vi]++;
        }
     
    }
}
