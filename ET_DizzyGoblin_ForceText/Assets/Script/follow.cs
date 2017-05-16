using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour {

    public Transform target;


    public float distance = 30;
    public float height = 5;
    public float interpRate = 10.0f;

    private Vector3 goalPos = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

        
        goalPos = target.position - (target.forward * distance);
        goalPos.y += height;

        transform.position = Vector3.Slerp(transform.position, goalPos, Time.deltaTime * interpRate);
        transform.LookAt(target.position);


    }
}
