﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class targetRightArmIdle : IKAnimationTarget
{

    //DONT FORGET TO RE-NAME IT, YOUR INITIALS, AND SOME LOGICAL NAME
    //the FSM can names of our creation to select animations to play
    /*
         "JK_rightArmIdle";
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
	}
	
	// Update is called once per frame
	void Update ()
    {
		speed = goblinGlobals.speed;
    
        //to keep our targets in line with the hips, we simply want to
        //oscillate on z axis in the LOCAL space. we will always want something
        //sort of like this:

        Vector3 lpos = transform.localPosition;
        lpos.Set( lpos.x, 
                  Mathf.Sin((Time.time * speed) + phase) * range, 
                  Mathf.Sin((Time.time * speed) + phase) * range);
        
        //set the local
        transform.localPosition = lpos;


        //to keep the target on the terrain surface
        /*
        //get the world 
        Vector3 pos = transform.position;

        float y = mesh.getHeightAt(pos);
        pos.y = y + heightOffset;

        //set the final position
        transform.position = pos;
        */

    }
}
