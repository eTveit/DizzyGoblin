using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class targetMoveTrees : MonoBehaviour {
	

    public TerrainMesh mesh = null;

    //phase determines the relationship between multiple move points
    //as a function of PI, as Sin is the oscillating function
    public float phase = 0;
    
    //how fast the target point moves
    public float speed = 1;

    //the range of motion of the move point
    public float range = 1; 
	

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update ()
    {
	
        //to keep our targets in line with the hips, we simply want to
        //oscillate on z axis in the LOCAL space

        Vector3 lpos = transform.localPosition;
        lpos.Set(lpos.x, lpos.y, Mathf.Sin((Time.time * speed) + phase) * range);


        //set the local
        transform.localPosition = lpos;
        
    

    }
}
