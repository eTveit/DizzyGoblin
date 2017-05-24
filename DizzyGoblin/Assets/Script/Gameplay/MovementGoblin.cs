using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGoblin : MonoBehaviour {


    public Transform terrain = null;
    public float heightOffset = 0;

    public float speed = 30.0f;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {

        float x = transform.position.x;
        float z = transform.position.z;


        //TODO: refactor this into a control FSM 
        //(see steering from last semester)
        if (Input.GetKey(KeyCode.A))
            x = x + speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
            x = x - speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
            z = z - speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.S))
            z = z + speed * Time.deltaTime;

        float y = transform.position.y;

        // place the dude
        //first  get the exact height
        float goalY = terrain.GetComponent<TerrainMesh>().getHeightAt(new Vector3(x, 1, z)) + heightOffset;

        Vector3 goalpos = new Vector3(x, goalY, z);

        //interp to position quickly
        //and set new position
        transform.position = Vector3.Lerp(transform.position, goalpos, Time.deltaTime * 10);



    }

    //this searches for a Transform specifically
    //TODO: make it search for anything, and cast it when it returns 
    public Transform Search(Transform target, string name)
    {
        if (target.name == name) return target;

        for (int i = 0; i < target.childCount; i++)
        {
            //we use "var" because the component could be anything
            var result = Search(target.GetChild(i), name);

            if (result != null) return result;
        }

        return null;
    }


}
    /*[Phong]Didn't get the code to work 
     * 
    public Transform terrain = null;
    public float heightOffset = 0;
    public float maxSpeed = 50;
    public float acceleration = 10;
    public float deceleration = 10;
    public float speed = 0;
    public float xSpeed;
    public float zSpeed;
      
        float dt = Time.deltaTime;

        float x = transform.position.x;
        float z = transform.position.z;


        //TODO: refactor this into a control FSM 
        //(see steering from last semester)

        if ((Input.GetKey(KeyCode.A)) && (xSpeed < maxSpeed))
            xSpeed = xSpeed - acceleration * dt;

        else if ((Input.GetKey(KeyCode.D)) && (xSpeed > -maxSpeed))
            xSpeed = xSpeed + acceleration * dt;

        if ((Input.GetKey(KeyCode.W)) && (zSpeed < maxSpeed))
            zSpeed = zSpeed - acceleration * dt;

        else if ((Input.GetKey(KeyCode.S)) && (zSpeed > -maxSpeed))
            zSpeed = zSpeed + acceleration * dt;
       
        else {
            if (xSpeed > deceleration * dt)
                xSpeed = xSpeed - deceleration * dt;

        else if (xSpeed < -deceleration * dt)
                xSpeed = xSpeed + deceleration * dt;

            if (zSpeed > deceleration * dt)
                zSpeed = zSpeed - deceleration * dt;

            else if (zSpeed < -deceleration * dt)
                zSpeed = zSpeed + deceleration * dt;

            else
            xSpeed = 0;
            zSpeed = 0;
        }

        float y = transform.position.y;

        // place the dude
        //first  get the exact height
        float goalY = terrain.GetComponent<TerrainMesh>().getHeightAt(new Vector3(x, 1, z)) + heightOffset;

        Vector3 goalpos = new Vector3(x, goalY, z);

        //interp to position quickly
        //and set new position


        transform.position = Vector3.Lerp(transform.position, goalpos, Time.deltaTime * 10);
        */
