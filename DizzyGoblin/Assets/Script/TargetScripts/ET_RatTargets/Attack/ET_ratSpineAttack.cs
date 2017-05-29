//copyright Espen Tveit 2017

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ET_ratSpineAttack : IKAnimationTarget {

    //DONT FORGET TO RE-NAME IT, YOUR INITIALS, AND SOME LOGICAL NAME
    //the FSM uses names of our creation to select animations to play
    /*
         "ET_ratSpineAttack";
    */
    public override string getAnimName() {
        return animationName;
    }

    Vector3[] positions;

    //we probably always want these references
    public Transform AvatarObj;
    public TerrainMesh mesh = null;
    private GoblinGlobals ratGlobals;
    public ET_ratRightArmAttack rightArmTarget;
    public ET_ratLeftArmAttack leftArmTarget;
    public ET_ratRightLegAttack rightLegTarget;
    public ET_ratLeftLegAttack leftLegTarget;
    public ET_ratTailAttack tailTarget;

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

    public float incrementingDT = -1;


    // Use this for initialization
    void Start() {


        positions = new Vector3[5];

        positions[0] = startPosition;
        positions[1] = new Vector3(3.25f, 7, 1.7f);
        positions[2] = new Vector3(-1.3f, 4.1f, 1.4f);
        positions[3] = new Vector3(-2.3f, 6.1f, 1.4f);
        positions[4] = new Vector3(1.7f, 3.7f, 1.2f);


        ratGlobals = AvatarObj.GetComponent<GoblinGlobals>();


        Segment3d thiL = ratGlobals.Search(AvatarObj, "Thigh_L").GetComponent<Segment3d>();
        Segment3d fooL = ratGlobals.Search(AvatarObj, "Foot_L").GetComponent<Segment3d>();
        Segment3d thiR = ratGlobals.Search(AvatarObj, "Thigh_R").GetComponent<Segment3d>();
        Segment3d fooR = ratGlobals.Search(AvatarObj, "Foot_R").GetComponent<Segment3d>();
        Segment3d spiM = ratGlobals.Search(AvatarObj, "Spine_Middle").GetComponent<Segment3d>();
        Segment3d spiT = ratGlobals.Search(AvatarObj, "Spine_Top").GetComponent<Segment3d>();
        Segment3d head = ratGlobals.Search(AvatarObj, "Head001").GetComponent<Segment3d>();

        thiL.Ycomp = -20;
        fooL.Ycomp = -90;
        thiR.Ycomp = -20;
        fooR.Ycomp = -90;
        spiM.Ycomp = -60;
        spiT.Ycomp = -30;
        head.Ycomp = -30;

        phase = 0;

        range = 2;

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

        if(dist < 0.1f)
            curPos++;

        if(curPos >= positions.Length)
            curPos = 0;

        rightArmTarget.curPos = curPos;
        leftArmTarget.curPos = curPos;
        rightLegTarget.curPos = curPos;
        leftLegTarget.curPos = curPos;
        tailTarget.curPos = curPos;

        //set the local
        transform.localPosition = lpos;

      
       

    }
}
