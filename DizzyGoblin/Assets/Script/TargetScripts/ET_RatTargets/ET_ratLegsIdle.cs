//copyright Espen Tveit 2017

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ET_ratLegsIdle : IKAnimationTarget {

    //DONT FORGET TO RE-NAME IT, YOUR INITIALS, AND SOME LOGICAL NAME
    //the FSM uses names of our creation to select animations to play
    /*
         "ET_ratLegsIdle";
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

    private float moveTime = -1;
    private float moveTimer = -1;

    private Vector3 goalPos = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start() {


        ratGlobals = AvatarObj.GetComponent<GoblinGlobals>();

        moveTime = UnityEngine.Random.Range(1, 3);
        moveTimer = 0;

        incrementingDT = 0;

        goalPos = transform.position;

    }

    // Update is called once per frame
    void Update() {

        //we need to smoothly transition to the new start point before running the animation
        if(interpolateToStartPosition(Time.deltaTime, speed) == false)
            return;

        speed = ratGlobals.speed/3;

        incrementingDT += Time.deltaTime;
        moveTimer += Time.deltaTime;

        Vector3 curPos = transform.position;
        

        //to keep our targets in line with the hips, we simply want to
        //oscillate on z axis in the LOCAL space
        
        if(moveTimer > moveTime) {
            float goalX = UnityEngine.Random.Range(startPosition.x - 0.3f, startPosition.x + 0.3f);
            float goalZ = UnityEngine.Random.Range(startPosition.z - 0.3f, startPosition.z + 0.3f);
            goalPos = new Vector3(goalX, 0, goalZ);
            moveTime = UnityEngine.Random.Range(3, 6);
            moveTimer = 0;
            transform.localPosition = goalPos;
            goalPos = transform.position;
            Debug.Log(transform.name);
        }

        

        //get the global, keep the target on the terrain surface
        
        

        transform.position = Vector3.Lerp(curPos, goalPos, Time.deltaTime * speed);

        //Stick the feet to the floor
        Vector3 pos = transform.position;
        float y = mesh.getHeightAt(pos);
        pos.y = y + heightOffset;

        //set the final position
        transform.position = pos;


    }
}
