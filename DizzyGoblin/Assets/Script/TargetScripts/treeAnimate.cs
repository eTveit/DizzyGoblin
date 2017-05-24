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

    float timer = -1.0f;
    float dirx = 1.0f;
    float diry = 1.0f;


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
        //if (interpolateToStartPosition(Time.deltaTime, speedX) == false)
        //	return;


        if (!terrain.built)
            return;

        Vector3 cpos = transform.localPosition;


        
        if (timer < 0)
            timer = Time.time;



        dirx = Mathf.Lerp(dirx, terrain.goaldirx, Time.deltaTime);
        diry = Mathf.Lerp(diry, terrain.goaldiry, Time.deltaTime);


        float xp = cpos.x - Mathf.Sin(Time.time) * Time.deltaTime * dirx * 0.4f;
        float zp = cpos.z - Mathf.Cos(Time.time) * Time.deltaTime * diry * 0.4f;

        Vector3 lpos = new Vector3(xp, cpos.y, zp);

        //interpolate to new pos
        transform.localPosition = lpos; // Vector3.Slerp(cpos,lpos,Time.deltaTime);




    }
}
