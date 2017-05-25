﻿//copyright Kitty Toft 2017

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KT_Goblin_RightLegIdle : IKAnimationTarget
{

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

    // Speed Modifier
    public float speedMod = 1.0f;

    // Keyframes and Keyframe Count
    Vector3[] keyframes = new[] { new Vector3(0.0f, 0.0f, 0.0f), new Vector3(-0.10f, 0.0f, 0.0f) };
    // Slow movement
    //Vector3[] keyframes = new[] { new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.5f, 0.0f, 0.0f) };
    private int currentFrame = 0;


    // Use this for initialization
    void Start()
    {

        goblinGlobals = AvatarObj.GetComponent<GoblinGlobals>();
    }

    // Update is called once per frame
    void Update()
    {

        //we need to smoothly transition to the new start point before running the animation
        if (interpolateToStartPosition(Time.deltaTime, speed) == false)
            return;



        speed = goblinGlobals.speed;

        //to keep our targets in line with the hips, we simply want to
        //oscillate on z axis in the LOCAL space

        Vector3 lpos = transform.localPosition;
        Vector3 goalPos = keyframes[currentFrame];

        //we need to transition smoothly from one target script to the next  
        float dist = Vector3.Distance(lpos, goalPos);

        //this threshold probably needs a tweak, if I am close enough, complete the task
        if (dist < 0.1f)
        {
            // current keyframe met, change to next one.
            currentFrame++;
            if (currentFrame == keyframes.Length)
            {
                // If the current frame indicator is higher than the number
                // of elements in the keyframes element, it should reset.
                currentFrame = 0;
            }
            return;
        }
        else
        {
            //move quickly to that position
            lpos = Vector3.Lerp(lpos, goalPos, Time.deltaTime * speed * speedMod);
        }


        //set the local
        transform.localPosition = lpos;

        //get the global, keep the target on the terrain surface
        Vector3 pos = transform.position;
        float y = mesh.getHeightAt(pos);
        pos.y = y + heightOffset;

        //here comes the interp to final position - we can use the object to perform the math
        //in the correct spatial context, then do the interpolation using the position when 
        //we first entered the Update(). Lerp or Slerp, depends on your preference
        //transform.position = Vector3.Lerp(curpos, pos, Time.deltaTime * speed);
        //NOTE: I hate not being able to pass my own delta time to the mono update function
        //      this means I have to rely upon a global variable to change "world" time for slo/fast mo


    }
}