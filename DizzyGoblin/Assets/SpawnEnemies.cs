using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {


	public TerrainMesh terrain;
	public int EnemyCount;
	// Use this for initialization
	void Start () {
		
	}



	// Update is called once per frame
	void Update ()
	{
	
		if(Input.GetKeyUp(KeyCode.E))
		{
					
				SpawnEnemy(EnemyCount);
		}
	}
	
	void SpawnEnemy(int howmany)
	{
	
		int c = 0;
		foreach (Transform child in transform) 
		{
			int x = Random.Range (100, terrain.xSize - 10);
			int z = Random.Range (100, terrain.zSize - 10);

			Vector3 pos = new Vector3 (x, 0, z);
			float y = terrain.getHeightAt (pos);

			pos.Set (pos.x, y, pos.z);
			c++;

			if (c >= howmany)
				return;

		}

	}
}
