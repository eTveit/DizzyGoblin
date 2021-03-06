﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class targetLeftArmIdle : IKAnimationTarget
{

    //DONT FORGET TO RE-NAME IT, YOUR INITIALS, AND SOME LOGICAL NAME
    //the FSM uses names of our creation to select animations to play
    /*
         "JK_idleLeftArm";
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


    float prevLAXcomp = 0;
    float prevLSXcomp = 0;
    Segment3d shoL = null;
    Segment3d armL = null;

    // Use this for initialization
    void Start()
    {

        goblinGlobals = AvatarObj.GetComponent<GoblinGlobals>();

        shoL = goblinGlobals.Search(AvatarObj, "Shoulder_R").GetComponent<Segment3d>();
        armL = goblinGlobals.Search(AvatarObj, "Arm_R").GetComponent<Segment3d>();

        //first buffer existing values
        prevLAXcomp = armL.Xcomp;
        prevLSXcomp = shoL.Xcomp;
        //set to what we want
        armL.Xcomp = 30;
        shoL.Xcomp = 30;

    }

    private void OnDisable()
    {
        //restore on disable
        armL.Xcomp = prevLAXcomp;
        shoL.Xcomp = prevLSXcomp;
    }

    // Update is called once per frame
    void Update ()
    {
		speed = goblinGlobals.speed;

        if (interpolateToStartPosition(Time.deltaTime, speed) == false)
            return;

        Vector3 curpos = transform.position;

        //to keep our targets in line with the hips, we simply want to
        //oscillate on z axis in the LOCAL space

        Vector3 lpos = transform.localPosition;
        lpos.Set( -3f + Mathf.Sin((Time.time * speed) + phase) * range,
                  1 + Mathf.Sin((Time.time * speed) + phase) * range,
                  0.5f + Mathf.Sin((Time.time * speed) + phase) * range);


        //set the local
        transform.localPosition = lpos;


        //get the world 
        Vector3 pos = transform.position;
        //to keep the target on the terrain surface
        /*
        float y = mesh.getHeightAt(pos);
        pos.y = y + heightOffset;
        */

        //here comes the interp to final position - we can use the object to perform the math
        //in the correct spatial context, then do the interpolation using the position when 
        //we first entered the Update(). Lerp or Slerp, depends on your preference
        transform.position = Vector3.Lerp(curpos, pos, Time.deltaTime * speed);
        //NOTE: I hate not being able to pass my own delta time to the mono update function
        //      this means I have to rely upon a global variable to change "world" time for slo/fast mo


    }
}
