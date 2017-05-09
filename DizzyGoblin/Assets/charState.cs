using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charState : MonoBehaviour {



    public Transform terrain = null;
    public float heightOffset = 0;

    public float speed = 2.5f;

    public float rotationSpeed = 0;
    public float rotationBoost = 0;

    public Transform lFootTarget = null;
    private targetMove lWalkingTarget = null;
    private targetMoveSpin lSpinningTarget = null;
    public Transform rFootTarget = null;
    private targetMove rWalkingTarget = null;
    private targetMoveSpin rSpinningTarget = null;

    private enum MovementState { walking, spinning };
    private MovementState myMovementState = MovementState.walking;

    // Use this for initialization
    void Start() {
        lWalkingTarget = lFootTarget.GetComponent<targetMove>();
        lSpinningTarget = lFootTarget.GetComponent<targetMoveSpin>();
        rWalkingTarget = rFootTarget.GetComponent<targetMove>();
        rSpinningTarget = rFootTarget.GetComponent<targetMoveSpin>();

        lWalkingTarget.enabled = true;
        lSpinningTarget.enabled = false;
        rWalkingTarget.enabled = true;
        rSpinningTarget.enabled = false;
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

        if(myMovementState == MovementState.spinning) {
            Rotate();
        }

        if(Input.GetKeyDown(KeyCode.Q)) {
            SwitchState();
        }
    }

    void SwitchState() {
        if(myMovementState == MovementState.walking) {
            myMovementState = MovementState.spinning;
            lWalkingTarget.enabled = false;
            lSpinningTarget.enabled = true;
            rWalkingTarget.enabled = false;
            rSpinningTarget.enabled = true;
        }
        else if(myMovementState == MovementState.spinning) {
            myMovementState = MovementState.walking;
            lWalkingTarget.enabled = true;
        lSpinningTarget.enabled = false;
        rWalkingTarget.enabled = true;
        rSpinningTarget.enabled = false;
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
