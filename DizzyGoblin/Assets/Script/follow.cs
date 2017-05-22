using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour {

    public Transform target;


    public float distance = 30;
    public float height = 5;
    public float interpRate = 10.0f;
    public float speed = 50.0f;

    private Vector3 goalPos = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start ()
    {
        Debug.Log("Move Camera around with arrow keys");
	}
    void Update()
    {
        //Revised by PVM
        float dt = Time.deltaTime;
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3((speed + 10) * dt, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3( -speed * dt, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, (-speed - 45) * dt, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, (speed - 30) * dt, 0));
        }
    }
    // Update is called once per frame
    void LateUpdate () {

                        // change - to +, to make the camera position behind the Goblin.
        goalPos = target.position - (target.forward * distance);
        goalPos.y += height;

        transform.position = Vector3.Slerp(transform.position, goalPos, Time.deltaTime * interpRate);
        transform.LookAt(target.position);


    }
}
