using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charState : MonoBehaviour {



    public Transform terrain = null;
    public float heightOffset = 0;

    public float speed = 2.5f;

    public float rotationSpeed = 0;
    public float rotationBoost = 0;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        float x = transform.position.x;
        float z = transform.position.z;
        


        //TODO: refactor this into a control FSM 
        //(see steering from last semester)
        if (Input.GetKey(KeyCode.LeftArrow))
            x = x + speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.RightArrow))
            x = x - speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.UpArrow))
            z = z - speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.DownArrow))
            z = z + speed * Time.deltaTime;

        float y = transform.position.y;       

        // place the dude
        //first  get the exact height
        float goalY = terrain.GetComponent<TerrainMesh>().getHeightAt( new Vector3(x, 1, z) ) + heightOffset;

        Vector3 goalpos = new Vector3(x, goalY, z);

        //interp to position quickly
        //and set new position
        transform.position = Vector3.Lerp(transform.position, goalpos, Time.deltaTime * 10);

        if(rotationSpeed != 0) {
            Rotate();
        }
    }

    void Rotate() {
        if(Mathf.Cos((Time.time * 4) + 3.141593f) > 0.9f) {
            rotationBoost = 50;
        }
        else {
            rotationBoost = 0;
        }

        // this is not physics, it should be rebuilt
        Quaternion rotation = Quaternion.AngleAxis(-(rotationSpeed + rotationBoost) * Time.deltaTime, transform.up);
        transform.rotation *= rotation;
    }
}
