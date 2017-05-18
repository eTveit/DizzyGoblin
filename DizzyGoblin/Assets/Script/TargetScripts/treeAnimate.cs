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


    public TerrainMesh terrain;
    public TreeGlobals treeGlobals;
    public Transform tree;

    //phase determines the relationship between multiple move points
    //as a function of PI, as Sin is the oscillating function
    public float phaseX = 7;
    
    //how fast the target point moves
    public float speedX = 1;

    //the range of motion of the move point
    public float rangeX = 1;


    //phase determines the relationship between multiple move points
    //as a function of PI, as Sin is the oscillating function
    public float phaseZ = 1;

    //how fast the target point moves
    public float speedZ = 1;

    //the range of motion of the move point
    public float rangeZ = 1;



    // Use this for initialization
    void Awake()
    {
        tree = GetComponentInParent<Transform>();
    }

    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {

		//we need to smoothly transition to the new start point before running the animation
		if (interpolateToStartPosition(Time.deltaTime, speedX) == false)
			return;


        Vector3 cpos = transform.localPosition;

        //to keep our targets in line with the hips, we simply want to
        //oscillate on z axis in the LOCAL space

        float xp = tree.position.x ;
        float zp = 0; //tree.position.z ;

        //xp and zp will need to have a coefficient of wind dir/speed

        float xm = Mathf.Sin((Time.time * speedX) + phaseX + xp);
        float zm = Mathf.Sin((Time.time * speedZ) + phaseX + zp);
        
        Vector3 lpos = transform.localPosition;
        lpos.Set(xm * rangeX, 10, zm * rangeZ);
        
        //interpolate to new pos
        transform.localPosition = Vector3.Slerp(cpos,lpos,Time.deltaTime);




    }
}
