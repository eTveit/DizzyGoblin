using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//@phong - teleport is the effect, rather than the cause, so perhaps this should be called 
//         "RatHit" rather than teleport.
 
public class TeleportObject : MonoBehaviour {

    //Script should be placed on rats

    //Put the rats spawn pool in Destination
	public Transform Destination;
    public Transform Target;
    float timer;
    public float DeSpawn = 3;
    bool Dead = false;
    steering steering;
    public float distance;
    public float distanceTrigger = 5;

    public bool isHitByBall = false;

	// Use this for initialization
	void Start ()
    {
        steering = GetComponent<steering>();
    }
	
	// Update is called once per frame
	void Update ()
    {
 
        distance = Vector3.Distance(transform.position, Target.position);
    
        timer += Time.deltaTime;
        HitByBall();
        if (Dead == true && timer > DeSpawn)
        {
            teleport();
            Dead = false;
            //The ideal would be to disable steering while they are in hell and enable it when they spawn. rather than enable it when they go to hell. Takes too much uneeded power.
            steering.enabled = true;
        }

        
    }

	void HitByBall()
	{
        //<JPK> we probaly also want to confirm that the goblin is spinning AND use a collision
        //      box with the ball and chain as an event handler.
		if (distance < distanceTrigger && !Dead)
		{
            timer = 0;
            Dead = true;
            //Disable steering script on rat so that it stays still when dead
            steering.enabled = false;
            isHitByBall = true;
        }
    }

    void teleport()
    {
        transform.position = Destination.position;
        isHitByBall = false;
    }
}
//Thieu Phong Le
