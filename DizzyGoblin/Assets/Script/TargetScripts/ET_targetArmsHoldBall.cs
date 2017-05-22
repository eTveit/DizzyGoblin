//copyright john klima 2017
//revised today espen

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ET_targetArmsHoldBall : IKAnimationTarget
{

    //DONT FORGET TO RE-NAME IT, YOUR INITIALS, AND SOME LOGICAL NAME
    //the FSM uses names of our creation to select animations to play
    /*
         "ET_targetArmsHoldBall";
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


    public float incrementingDT = -1;


    // Use this for initialization
    void Start () {
        
		goblinGlobals = AvatarObj.GetComponent<GoblinGlobals> ();

        range = 0.2f;

        incrementingDT = 0;

	}
	
	// Update is called once per frame
	void Update ()
    {
        incrementingDT += Time.deltaTime;

        speed = goblinGlobals.speed;

        //we need to smoothly transition to the new start point before running the animation
        if (interpolateToStartPosition(Time.deltaTime, speed) == false)
			return;

	



        Vector3 lpos = transform.localPosition;
        lpos.Set(startPosition.x, startPosition.y + Mathf.Sin((incrementingDT * speed) + phase) * range, startPosition.z + Mathf.Sin((incrementingDT * speed) + phase) * range);
        


        //lpos.Set(lpos.x, lpos.y, Mathf.Sin((Time.time * speed) + phase) * range);


        //set the local
        transform.localPosition = lpos;
        
        //get the global, keep the target on the terrain surface
		/*
        Vector3 pos = transform.position;
        float y = mesh.getHeightAt(pos);
        pos.y = y + heightOffset;
        
        //set the final position
        transform.position = pos;
		*/

    }
}
