//Made by Kittikorn Detnoi
//Edited by Lars Joar Bjørkeland. Attached it to LevelManager

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleSpawn : MonoBehaviour {

    public LJB_levelManager levelManager;

	// Use this for initialization
	public void Spawn (int num_Rats) {

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
