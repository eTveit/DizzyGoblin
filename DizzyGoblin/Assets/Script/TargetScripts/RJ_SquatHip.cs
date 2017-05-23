using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RJ_SquatHip : MonoBehaviour {

    public float heightOffset = 1.166f;
    public Vector3 hipRotation = new Vector3(150.077f, 0, 0);
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(0, heightOffset, 0);
        Quaternion rot = Quaternion.Euler(hipRotation);

    }
}
