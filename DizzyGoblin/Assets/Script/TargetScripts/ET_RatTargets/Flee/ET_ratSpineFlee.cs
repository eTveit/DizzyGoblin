﻿//copyright Espen Tveit 2017

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ET_ratSpineFlee : IKAnimationTarget {

    //DONT FORGET TO RE-NAME IT, YOUR INITIALS, AND SOME LOGICAL NAME
    //the FSM uses names of our creation to select animations to play
    /*
         "ET_ratSpineFlee";
    */
    public override string getAnimName() {
        return animationName;
    }

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

    public float incrementingDT = -1;


    // Use this for initialization
    void Start() {


        ratGlobals = AvatarObj.GetComponent<GoblinGlobals>();


        Segment3d thiL = ratGlobals.Search(AvatarObj, "Thigh_L").GetComponent<Segment3d>();
        Segment3d fooL = ratGlobals.Search(AvatarObj, "Foot_L").GetComponent<Segment3d>();
        Segment3d thiR = ratGlobals.Search(AvatarObj, "Thigh_R").GetComponent<Segment3d>();
        Segment3d fooR = ratGlobals.Search(AvatarObj, "Foot_R").GetComponent<Segment3d>();
        Segment3d spiM = ratGlobals.Search(AvatarObj, "Spine_Middle").GetComponent<Segment3d>();
        Segment3d spiT = ratGlobals.Search(AvatarObj, "Spine_Top").GetComponent<Segment3d>();
        Segment3d head = ratGlobals.Search(AvatarObj, "Head001").GetComponent<Segment3d>();

        thiL.Ycomp = -50;
        fooL.Ycomp = -90;
        thiR.Ycomp = -50;
        fooR.Ycomp = -90;
        spiM.Ycomp = -60;
        spiT.Ycomp = -30;
        head.Ycomp = 0;

        phase = 0;

        range = 0.5f;

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

        Vector3 curPos = transform.position;

        //to keep our targets in line with the hips, we simply want to
        //oscillate on z axis in the LOCAL space

        Vector3 lpos = transform.localPosition;

        lpos.Set(startPosition.x + (Mathf.Sin((incrementingDT * speed) + phase) * range), startPosition.y + Mathf.Abs(Mathf.Sin((incrementingDT * speed) + phase+Mathf.PI/2) * range), startPosition.z);


        //lpos.Set(lpos.x, lpos.y, Mathf.Sin((Time.time * speed) + phase) * range);


        //set the local
        transform.localPosition = lpos;

        //get the global, keep the target on the terrain surface
        Vector3 pos = transform.position;
        

        transform.position = Vector3.Lerp(curPos, pos, Time.deltaTime * speed);


    }
}
