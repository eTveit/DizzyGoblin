using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class targetMoveRight : IKAnimationTarget
{

    //copyright Peter Vestnes Måseidvåg 2017
    //the FSM uses names of our creation to select animations to play
    /*
         "Pvm_walkright";
    */
    public override string getAnimName()
    {
        return animationName;
    }
    

    //we probably always want these references
    public Transform AvatarObj;
    public TerrainMesh mesh = null;
    private GoblinGlobals goblinGlobals;
    public Transform leftFoot;
    




    //phase determines the relationship between multiple move points
    //as a function of PI, as Sin is the oscillating function
    public float phase = 0;
    
    //how fast the target point moves
    public float speed = 1;

    //the range of motion of the move point
    public float range = 1; 

    //how high the movepoint sits above terrain surface
    public float heightOffset = 0;
    public float MinX = 1;
    public float MaxX = 1;

    public int cycleCount = 0;    

	// Use this for initialization
	void Start () {

		goblinGlobals = AvatarObj.GetComponent<GoblinGlobals> ();
	}
	
	// Update is called once per frame
	void Update ()
    {
        float dt = Time.deltaTime;
        speed = goblinGlobals.speed;
       

        //<JK>  added new requirement to move to a start position
        //      this will help us transition animations
        if (interpolateToStartPosition(Time.deltaTime, speed) == false)
            return;

        Vector3 curpos = transform.position;

        Vector3 rpos = transform.localPosition;

        float cycleX = Mathf.Sin((Time.time * speed) + phase) * range;

        rpos.Set(cycleX + (Mathf.Clamp(transform.position.x, MinX, MaxX)), rpos.y, rpos.z);

        if (cycleX > 0.99f)
            cycleCount++;
        
        
        //Restriciting movement

        transform.localPosition = rpos;

        Vector3 pos = transform.position;
        float y = mesh.getHeightAt(pos);
        pos.y = y + heightOffset;

        //here comes the interp to final position - we can use the object to perform the math
        //in the correct spatial context, then do the interpolation using the position when 
        //we first entered the Update(). Lerp or Slerp, depends on your preference
        transform.position = Vector3.Lerp(curpos, pos, Time.deltaTime * speed);
        //NOTE: I hate not being able to pass my own delta time to the mono update function
        //      this means I have to rely upon a global variable to change "world" time for slo/fast mo

        /*if (leftFoot)
        {
            float dist = Vector3.Distance(leftFoot.position, transform.position);
            print("Distance to LeftFoot: " + dist);
          
        }
        */






    }



}
