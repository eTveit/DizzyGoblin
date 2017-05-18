using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObject : MonoBehaviour {

	public Transform Destination;
    float timer;
    public float DeSpawn = 3;
    bool Dead = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if (Dead == true && timer > DeSpawn)
        {
            teleport();
            Dead = false;
        }
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Ball")
		{
            timer = 0;
            Dead = true;
            
		}
	}

    void teleport()
    {
        transform.position = Destination.position;
    }
}
