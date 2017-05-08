using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateCenter : MonoBehaviour {


    public float phase = Mathf.PI ;
    public float range = Mathf.PI / 2;
    public float center = -Mathf.PI / 5;
    public float speed = 1;

    public Quaternion rot;

    // Use this for initialization
    void Start ()
    {
        //get the initial rotation
        rot = transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {

        rot = Quaternion.Euler(Vector3.forward *  (Mathf.Sin(Time.time + phase) * range + center) );

        transform.rotation = rot;

    }
}
