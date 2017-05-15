using System;
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
        

	// Use this for initialization
	void Start () {

        //get the avatar global properties
		goblinGlobals = AvatarObj.GetComponent<GoblinGlobals> ();

		Segment3d shoL = goblinGlobals.Search(AvatarObj, "Shoulder_L").GetComponent<Segment3d>();
		Segment3d armL = goblinGlobals.Search(AvatarObj, "Arm_L").GetComponent<Segment3d>();
		armL.Xcomp = -30;
		shoL.Xcomp = -30;

        //probably want to set some initial positions, and we probably want
        //to INTERPOLATE to those start positions....
	}
	
	// Update is called once per frame
	void Update ()
    {
		speed = goblinGlobals.speed;
    
        //to keep our targets in line with the hips, we simply want to
        //oscillate on z axis in the LOCAL space

        Vector3 lpos = transform.localPosition;
		lpos.Set(-1.5f + Mathf.Sin((Time.time * speed) + phase) * range,
				 0.5f + Mathf.Sin((Time.time * speed) + phase) * range,
				 0.5f + Mathf.Sin((Time.time * speed) + phase) * range);


		//set the local
		transform.localPosition = lpos;



        //to keep the target on the terrain surface
        //get the world 
        /*
        Vector3 pos = transform.position;

        float y = mesh.getHeightAt(pos);
        pos.y = y + heightOffset;

        //set the final position
        transform.position = pos;
        */

    }
}
