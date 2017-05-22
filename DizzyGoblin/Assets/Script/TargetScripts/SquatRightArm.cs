using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SquatRightArm : IKAnimationTarget
{

    //DONT FORGET TO RE-NAME IT, YOUR INITIALS, AND SOME LOGICAL NAME
    //the FSM uses names of our creation to select animations to play
    /*
         "JK_walk";
    */
    public override string getAnimName()
    {
        return animationName;
    }

    Vector3[] positions;

    //we probably always want these references
    public Transform AvatarObj;
    public TerrainMesh mesh = null;
    private GoblinGlobals goblinGlobals;

    //phase determines the relationship between multiple move points
    //as a function of PI, as Sin is the oscillating function
    public float phase = 0;
    
    //how fast the target point moves
    public float speed = 1;

    //the range of motion of the move point
    public float range = 1; 

    //how high the movepoint sits above terrain surface
    public float heightOffset = 0;

    private int curPos = 0;

    public bool squatState = false; 

	// Use this for initialization
	void Start () {

		goblinGlobals = AvatarObj.GetComponent<GoblinGlobals> ();

        positions = new Vector3[2];

        positions[0] = new Vector3(2.18f, 1.76f, 0.1f);
        positions[1] = new Vector3(0.398f, 1.827f, 1.758f);


        //positions[0] = new Vector3(2.18f, 1.76f, 0.1f);
        //positions[1] = new Vector3(0.398f, 1.827f, 1.758f);


    }

    // Update is called once per frame
    void Update ()
    {

		//we need to smoothly transition to the new start point before running the animation
		//if (interpolateToStartPosition(Time.deltaTime, speed) == false)
			//return;

		speed = goblinGlobals.speed;

        Vector3 lpos = transform.localPosition;

        Vector3 goalPos = positions[curPos];

        float dist = Vector3.Distance(lpos, goalPos);



        lpos = Vector3.Slerp(lpos, goalPos, Time.deltaTime);


        transform.localPosition = lpos;


        if (dist < 0.1f && curPos < positions.Length - 1)
            curPos++;
        else if (squatState && dist < 0.1f )
            curPos++;

        if (curPos >= positions.Length && squatState == true)
            curPos = 0;



    }
}
