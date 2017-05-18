using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleSpawn : MonoBehaviour {

	// Use this for initialization
	public void Spawn () {

        foreach (Transform rat in transform)
        {

            int x = Random.Range(10, 120);
            int z = Random.Range(10, 120);

            rat.position = new Vector3(x, 1, z);
			rat.gameObject.SetActive(true);

        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
