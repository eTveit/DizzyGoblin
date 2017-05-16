using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class treeAnimate : IKAnimationTarget
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
    
    
    //phase determines the relationship between multiple move points
    //as a function of PI, as Sin is the oscillating function
    public float phase = 0;
    
    //how fast the target point moves
    public float speed = 1;

    //the range of motion of the move point
    public float range = 1; 

        

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {

		//we need to smoothly transition to the new start point before running the animation
		if (interpolateToStartPosition(Time.deltaTime, speed) == false)
			return;

	
        //to keep our targets in line with the hips, we simply want to
        //oscillate on z axis in the LOCAL space

        Vector3 lpos = transform.localPosition;
        lpos.Set(Mathf.Sin((Time.time * speed) + phase) * range, lpos.y, Mathf.Sin((Time.time * speed * 2) + phase) * range);


        //set the local
        transform.localPosition = lpos;
        



    }
}
