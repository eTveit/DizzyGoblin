using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ET_targetArmsHitTree : targetMove {
    //DONT FORGET TO NAME IT, YOUR INITIALS, AND SOME LOGICAL NAME
    //the FSM uses names of our creation to select animations to play
    //you can do it here, or in the editor. it is best to do it here
    //so when it is intantiated, it uses this name as the default
    /*
         "ET_targetArmsHitTree";
    */

    public override string getAnimName() {
        return animationName;
    }


    //for circular movement
    public float incrementingDT = -1;


    private GoblinGlobals goblinGlobals = null;

    // Use this for initialization
    void Start() {
        goblinGlobals = AvatarObj.GetComponent<GoblinGlobals>();

        phase = 0;


        range = 1.8f;

        incrementingDT = 0;
    }

    // Update is called once per frame
    void Update() {


        incrementingDT += Time.deltaTime;

        speed = goblinGlobals.speed * 2.2f;

        //<JK> we need to smoothly transition to the new start point before running the animation
        if(interpolateToStartPosition(Time.deltaTime, speed) == false)
            return;

        //<JK> we also probably always want to interpolate target positions in general, less jitter.
        Vector3 curpos = transform.position;
        

        //to keep our targets in line with the hips, we simply want to
        //oscillate on z axis in the LOCAL space

        Vector3 lpos = transform.localPosition;
        lpos.Set(startPosition.x, startPosition.y + Mathf.Cos((incrementingDT * speed) + phase + Mathf.PI) * range, Mathf.Sin((incrementingDT * speed) + phase) * range);
        

        //set the local
        transform.localPosition = lpos;

        //get the global, keep the target on the terrain surface
        Vector3 pos = transform.position;

        //here comes the interp to final position - we can use the object to perform the math
        //in the correct spatial context, then do the interpolation using the position when 
        //we first entered the Update(). Lerp or Slerp, depends on your preference
        transform.position = Vector3.Lerp(curpos, pos, Time.deltaTime * speed);
        //NOTE: I hate not being able to pass my own delta time to the mono update function
        //      this means I have to rely upon a global variable to change "world" time for slo/fast mo

    }
}
