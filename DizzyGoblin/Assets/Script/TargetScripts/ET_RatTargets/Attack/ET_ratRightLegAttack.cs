﻿//copyright Espen Tveit 2017

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ET_ratRightLegAttack : IKAnimationTarget {

    //DONT FORGET TO RE-NAME IT, YOUR INITIALS, AND SOME LOGICAL NAME
    //the FSM uses names of our creation to select animations to play
    /*
         "ET_ratRightLegAttack";
    */
    public override string getAnimName() {
        return animationName;
    }

    Vector3[] positions;

    //we probably always want these references
    public Transform AvatarObj;
    public TerrainMesh mesh = null;
    private GoblinGlobals ratGlobals;

    //phase determines the relationship between multiple move points
    //as a function of PI, as Sin is the oscillating function
    public float phase = 0;

    //how fast the target point moves
    public float speed = 1;

    //the range of motion of the move point
    public float range = 1;

    //how high the movepoint sits above terrain surface
    public float heightOffset = 0;

    public int curPos = 0;

    public float incrementingDT = -1;


    // Use this for initialization
    void Start() {


        positions = new Vector3[5];

        positions[0] = startPosition;
        positions[1] = new Vector3(0.65f, 0, -0.55f);
        positions[2] = new Vector3(0.1f, 0, 0f);
        positions[3] = startPosition;
        positions[4] = new Vector3(0.5f, 0, -0.2f);


        ratGlobals = AvatarObj.GetComponent<GoblinGlobals>();

        phase = 0;

        incrementingDT = 0;
        
    }

    // Update is called once per frame
    void Update() {

        speed = ratGlobals.speed * 2;

        //we need to smoothly transition to the new start point before running the animation
        if(interpolateToStartPosition(Time.deltaTime, speed) == false)
            return;

        speed = ratGlobals.speed*2;

        incrementingDT += Time.deltaTime;

        //to keep our targets in line with the hips, we simply want to
        //oscillate on z axis in the LOCAL space

        Vector3 lpos = transform.localPosition;

        Vector3 goalPos = positions[curPos];

        float dist = Vector3.Distance(lpos, goalPos);

        //we can use an adjust to interp FASTER the closer we are to the goal
        float adjust = (6.0f - dist);

        //if it is too small, clamp it, otherwise we wont get anywhere
        if(adjust < 0.5f)
            adjust = 0.5f;

        

        lpos = Vector3.Slerp(lpos, goalPos, Time.deltaTime * adjust);

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
