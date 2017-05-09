using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//  ||
public class MakeTrees : MonoBehaviour {

    public TerrainMesh terrain = null;
    
    public bool[] occupied;
	// Use this for initialization
	void BuildTrees ()
    {

		if (terrain == null)
			return;

		if (terrain.mesh == null)
			return;

        Mesh mesh = terrain.mesh;
        int vc = mesh.vertexCount;

       
        occupied = new bool[vc];


		foreach (Transform child in transform)
        {

			int x, z;
            float y;


            x = Random.Range(1, terrain.xSize);
            z = Random.Range(1, terrain.zSize);
            y = 0;

			int vi = getIndexFromXZ(x, z);

			int errorcount = 0;
            while (occupied[vi] || x % 2 == 0 || z % 2 == 0 )
            {

                Debug.Log("try again");

                x = Random.Range(1, terrain.xSize);
                z = Random.Range(1, terrain.zSize);
                y = 0;

                //check if this point is occupied
                vi = getIndexFromXZ(x, z);

				//just in case...
				errorcount++;
				if (errorcount > 1000)
					return;
            }

            occupied[vi] = true;

            Vector3 pos = new Vector3((float)x, y, (float)z);

            y = terrain.getHeightAt(pos) + 2.0f;

            pos.Set(x, y, z);

            child.transform.position = pos;
			

        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyUp(KeyCode.T))
            BuildTrees();	
	}

	int getIndexFromXZ(int x, int z)
	{
		if (terrain == null)
			return 0;

		return z * (terrain.xSize + 1) + x;
	}
}
