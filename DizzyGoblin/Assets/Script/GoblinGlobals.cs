using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinGlobals : MonoBehaviour {
    

    public Transform terrain = null;
    public float heightOffset = 0;

    public float speed = 2.5f;
    public bool isGoblin = false;
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {


        if (!isGoblin)
            return;

        float x = transform.position.x;
        float z = transform.position.z;

    
        //TODO: refactor this into a control FSM 
        //(see steering from last semester)
        if(Input.GetKey(KeyCode.LeftArrow))
            x = x + speed * Time.deltaTime;

        if(Input.GetKey(KeyCode.RightArrow))
            x = x - speed * Time.deltaTime;

        if(Input.GetKey(KeyCode.UpArrow))
            z = z - speed * Time.deltaTime;

        if(Input.GetKey(KeyCode.DownArrow))
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
