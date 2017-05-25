using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		if (distance < distanceTrigger && !Dead)
		{
            timer = 0;
            Dead = true;
            //Disable steering script on rat so that it stays still when dead
            steering.enabled = false;
        }
    }

    void teleport()
    {
        transform.position = Destination.position;
    }
}
//Thieu Phong Le
