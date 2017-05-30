//copyright Espen Tveit 2017

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ET_ratArmsIdle : IKAnimationTarget {

    //DONT FORGET TO RE-NAME IT, YOUR INITIALS, AND SOME LOGICAL NAME
    //the FSM uses names of our creation to select animations to play
    /*
         "ET_ratArmsIdle";
    */
    public override string getAnimName() {
        return animationName;
    }

    //we probably always want these references
    public Transform AvatarObj;
    public TerrainMesh mesh = null;
    private GoblinGlobals ratGlobals;

    //how fast the target point moves
    public float speed = 1;

    //the range of motion of the move point
    public float range = 1;
    

    private float moveTime = -1;
    private float moveTimer = -1;

    private Vector3 goalPos = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start() {


        ratGlobals = AvatarObj.GetComponent<GoblinGlobals>();

        moveTime = UnityEngine.Random.Range(1, 3);
        moveTimer = 0;
        

        goalPos = transform.position;

    }

    // Update is called once per frame
    void Update() {

        //we need to smoothly transition to the new start point before running the animation
        if(interpolateToStartPosition(Time.deltaTime, speed) == false)
            return;

        speed = ratGlobals.speed/3;
        
        moveTimer += Time.deltaTime;

        Vector3 curPos = transform.position;
        

        //to keep our targets in line with the hips, we simply want to
        //oscillate on z axis in the LOCAL space
        
        if(moveTimer > moveTime) {
            float goalX = UnityEngine.Random.Range(startPosition.x - 0.3f, startPosition.x + 0.3f)*range;
            float goalY = UnityEngine.Random.Range(startPosition.y - 0.3f, startPosition.y + 0.3f)*range;
            float goalZ = UnityEngine.Random.Range(startPosition.z - 0.3f, startPosition.z + 0.3f)*range;
            goalPos = new Vector3(goalX, goalY, goalZ);
            moveTime = UnityEngine.Random.Range(3, 6);
            moveTimer = 0;
            transform.localPosition = goalPos;
            goalPos = transform.position;
        }

        
        transform.position = Vector3.Lerp(curPos, goalPos, Time.deltaTime * speed);

       
        

    }
}
