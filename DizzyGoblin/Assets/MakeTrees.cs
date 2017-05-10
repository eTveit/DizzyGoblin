using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//  ||
public class MakeTrees : MonoBehaviour {

    public TerrainMesh terrain;
    
    public bool[] occupied;
    // Use this for initialization

        void BuildTrees ()
    {
               
        Mesh mesh = terrain.mesh;
        int vc = mesh.vertexCount;

       
        occupied = new bool[vc];
        int min = Random.Range(1, terrain.xSize);
        int max = min;

        foreach (Transform child in transform)
        {

            int x, z;
            float y;

                x = Random.Range(min, max);
                z = Random.Range(min, max);
                y = 0;

            
            
            int vi = z * (terrain.xSize + 1) + x;


            int errorcount = 0;
            while (occupied[vi]/* || x % 2 == 0 || z % 2 == 0*/)
            {

                Debug.Log("try again");

                x = Random.Range(min, max);
                z = Random.Range(min, max);
                y = 0;

                //check if this point is occupied
                vi = z * (terrain.xSize + 1) + x ;
                errorcount++;

                if (errorcount > 1000)
                {
                    return;
                }
            }

            occupied[vi] = true;

            Vector3 pos = new Vector3((float)x, y, (float)z);

            y = terrain.getHeightAt(pos) + 2.0f;

            pos.Set(x, y, z);

            child.transform.position = pos;

            min--;
            max++;

            if (min < 1)
            {
                min = 1;
            }
            if (max > terrain.xSize)
            {
                max = terrain.xSize;
            }
        }
    }

    void BuildFence()
    {
        Mesh mesh = terrain.mesh;
        int vc = mesh.vertexCount;

        int x, z;
        float y;

        x = 1;
        z = 1;
        y = 0;

        foreach (Transform child in transform)
        {

       
            int vi = z * (terrain.xSize + 1) + x;

            Vector3 pos = new Vector3((float)x, y, (float)z);

            y = terrain.getHeightAt(pos) + 2.0f;

            pos.Set(x, y, z);

            child.transform.position = pos;

                //check if this point is occupied
                vi = z * (terrain.xSize + 1) + x;

                //Place trees on coordinates up to 99,1
                if (x < 99 && z == 1)
                {
                    x++;
                }
                //Place trees on coordinates up to 99,99
                else if (x == 99 && z < 98)
                {
                    z++;
                }
                //Place trees on coordinates down to 1,99
                else if (x > 1 && z == 98)
                {
                    x--;
                }
                //Place trees on coordinates down to 1,1
                else if (x == 1 && z > 1)
                {
                    z--;
                }
            }
        }

        // Update is called once per frame
        void Update ()
    {
        if (Input.GetKeyUp(KeyCode.T))
            BuildFence();	
	}
}
