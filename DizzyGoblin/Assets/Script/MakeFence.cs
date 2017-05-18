using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  || Made by Lars Joar Bjørkeland

public class MakeFence : MonoBehaviour {

    public TerrainMesh terrain;

    // Use this for initialization


    //<JK> made public so it is called from level manager
    public void BuildFence()
    {
        Mesh mesh = terrain.mesh;
        int vc = mesh.vertexCount;

        int x, z;
        float y;

        x = 1;
        z = 1;
        y = 0;

        int rangeX = terrain.xSize - 1;
        int rangeZ = terrain.zSize - 1;

        foreach (Transform child in transform)
        {
            terrain.getVertexIndexFromXZ(x, z);
       
            //terrain supports this
            int vi = terrain.getVertexIndexFromXZ(x, z);  // z * (terrain.xSize + 1) + x;


            int yr = Random.Range(1, 360);
            Vector3 euler = new Vector3(0, yr, 0);
            Quaternion rot = Quaternion.Euler(euler);
            child.rotation = rot;

            Vector3 pos = new Vector3((float)x, y, (float)z);

            y = terrain.getHeightAt(pos) + 2.0f;

            pos.Set(x, y, z);

            child.transform.position = pos;

            //check if this point is occupied
            vi = terrain.getVertexIndexFromXZ(x, z);  // z * (terrain.xSize + 1) + x;

            //Place trees on coordinates up to 99,1
            if (x < rangeX && z == 1)
            {
                x += 2;
            }
            //Place trees on coordinates up to 99,99
            else if (x == rangeX && z < rangeZ)
            {
                z += 2;
            }
            //Place trees on coordinates down to 1,99
            else if (x > 1 && z == rangeZ)
            {
                x -= 2;
            }
            //Place trees on coordinates down to 1,1
            else if (x == 1 && z > 1)
            {
                z -= 2;
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
