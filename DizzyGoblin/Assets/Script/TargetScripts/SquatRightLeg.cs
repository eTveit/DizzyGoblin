﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SquatRightLeg : IKAnimationTarget
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
        

	// Use this for initialization
	void Start () {

		goblinGlobals = AvatarObj.GetComponent<GoblinGlobals> ();

        Segment3d thighR = goblinGlobals.Search(AvatarObj, "Thigh_R").GetComponent<Segment3d>();
        Segment3d calfR = goblinGlobals.Search(AvatarObj, "Calf_R").GetComponent<Segment3d>();
        Segment3d footR = goblinGlobals.Search(AvatarObj, "Foot_R").GetComponent<Segment3d>();


 
        thighR.Ycomp = 30;
        calfR.Ycomp = -30;
        footR.Ycomp = -20;
       
        
    }
	
	// Update is called once per frame
	void Update ()
    {

		//we need to smoothly transition to the new start point before running the animation
		if (interpolateToStartPosition(Time.deltaTime, speed) == false)
			return;

		speed = goblinGlobals.speed;


    
       //to keep our targets in line with the hips, we simply want to
        //oscillate on z axis in the LOCAL space

        Vector3 lpos = transform.localPosition;
        lpos.Set(0.405f, -0.05f, 0.133f);


        //set the local
        transform.localPosition = lpos;
        
        //get the global, keep the target on the terrain surface
        Vector3 pos = transform.position;
        float y = mesh.getHeightAt(pos);
        pos.y = y + heightOffset;
        
        //set the final position
        transform.position = pos;
        

    }
}
