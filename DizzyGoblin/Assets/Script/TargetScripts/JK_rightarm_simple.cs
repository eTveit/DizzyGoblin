//copyright john klima 2017
//revised today espen

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class JK_rightarm_simple: IKAnimationTarget
{

	//DONT FORGET TO RE-NAME IT, YOUR INITIALS, AND SOME LOGICAL NAME
	//the FSM uses names of our creation to select animations to play
	/*
         "JK_rightarm_simple";
    */
	public override string getAnimName()
    {
        return animationName;
    }


	Vector3[] positions;


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
        
	private int curPos = 0;

	// Use this for initialization
	void Start () {

		positions = new Vector3[2];

		positions[0] = new Vector3(2.3f, 3.6f, 0.86f);
		positions[1] = new Vector3(2.7f, 2.56f, 3.86f);
		

		goblinGlobals = AvatarObj.GetComponent<GoblinGlobals> ();

		Segment3d shoR = goblinGlobals.Search(AvatarObj, "Shoulder_R").GetComponent<Segment3d>();
		Segment3d armR = goblinGlobals.Search(AvatarObj, "Arm_R").GetComponent<Segment3d>();
		Segment3d elbR = goblinGlobals.Search(AvatarObj, "Elbow_R").GetComponent<Segment3d>();
		Segment3d handR = goblinGlobals.Search(AvatarObj, "Hand_R").GetComponent<Segment3d>();

		shoR.Xcomp = 30;
		shoR.Ycomp = -20;
		elbR.Xcomp = 20;
		handR.Xcomp = 40;
		handR.Zrange = 0.5f;




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

		Vector3 goalPos = positions[curPos];

		float dist = Vector3.Distance(lpos, goalPos);

		//we can use an sdjust to interp FASTER the closer we are to the goal
		float adjust = (3.0f - dist);

		//if it is too small, clamp it, otherwise we wont get anywhere
		if (adjust < 0.5f)
			adjust = 0.5f;

		lpos = Vector3.Slerp(lpos, goalPos, Time.deltaTime * adjust );

		if (dist < 0.1f)
			curPos++;

		if (curPos >= positions.Length)
			curPos = 0;
		
		
		
		//lpos.Set(lpos.x, lpos.y, Mathf.Sin((Time.time * speed) + phase) * range);


        //set the local
        transform.localPosition = lpos;
        
        //get the global, keep the target on the terrain surface
		/*
        Vector3 pos = transform.position;
        float y = mesh.getHeightAt(pos);
        pos.y = y + heightOffset;
        
        //set the final position
        transform.position = pos;
		*/

    }
}
