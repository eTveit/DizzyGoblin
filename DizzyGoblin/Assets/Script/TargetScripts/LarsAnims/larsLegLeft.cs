using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class larsLegLeft : IKAnimationTarget
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


    //we probably always want these references
    public Transform AvatarObj;
    public TerrainMesh mesh = null;
    private GoblinGlobals goblinGlobals;
    int DeathIndex;

    Vector3[] RandomDeath = new[] { new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f) };

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
    void Start()
    {

        goblinGlobals = AvatarObj.GetComponent<GoblinGlobals>();
        DeathIndex = UnityEngine.Random.Range(0, RandomDeath.Length);
    }

    // Update is called once per frame
    void Update()
    {

        //we need to smoothly transition to the new start point before running the animation
      //  if (interpolateToStartPosition(Time.deltaTime, speed) == false)
        //    return;

        Vector3 curpos = transform.position;

        speed = goblinGlobals.speed;

        //to keep our targets in line with the hips, we simply want to
        //oscillate on z axis in the LOCAL space

        Vector3 lpos = transform.localPosition;
       // Vector3 goalPos = DeathIndex;


        //set the local
        transform.localPosition = lpos;

        //get the global, keep the target on the terrain surface
        Vector3 pos = transform.position;
        float y = mesh.getHeightAt(pos);
        pos.y = y + heightOffset;

        //here comes the interp to final position - we can use the object to perform the math
        //in the correct spatial context, then do the interpolation using the position when 
        //we first entered the Update(). Lerp or Slerp, depends on your preference
        transform.position = Vector3.Lerp(curpos, pos, Time.deltaTime * speed);
        //NOTE: I hate not being able to pass my own delta time to the mono update function
        //      this means I have to rely upon a global variable to change "world" time for slo/fast mo


    }
}
