//Made by Lars Joar Bjørkeland

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTrees : MonoBehaviour
{

    public TerrainMesh terrain;
    public LJB_levelManager levelManager;


    private int treeCount = 0;
    
    
    // Use this for initialization

    public void BuildTrees(int num_trees)
    {

        treeCount = 0;

        Mesh mesh = terrain.mesh;
        int vc = mesh.vertexCount;


        // occupied = new bool[vc];

        //initial position/range
        //<JK>  the clustering is too clustered, I think maybe better to spread evenly
        //      keeping them 20 units from the edge
        int min = 20;//Random.Range(20, terrain.xSize - 20);
        int max = terrain.xSize - 20; //min ;

        Vector3 hell = new Vector3(-666, -666, -666);

        foreach (Transform child in transform)
        {

            if (treeCount >= num_trees)
            {
                child.transform.position = hell;
                child.gameObject.SetActive(false);
            }
            else
            {

                child.gameObject.SetActive(true);

                int x, z;
                float y;

                x = Random.Range(min, max);
                z = Random.Range(min, max);
                y = 0;



                int vi = terrain.getVertexIndexFromXZ(x, z); // z * (terrain.xSize + 1) + x;


                int errorcount = 0;
                while (levelManager.occupied[vi] || x % 2 == 0 || z % 2 == 0)
                {

                    Debug.Log("try again");

                    x = Random.Range(min, max);
                    z = Random.Range(min, max);
                    y = 0;

                    //check if this point is occupied
                    vi = terrain.getVertexIndexFromXZ(x, z); // z * (terrain.xSize + 1) + x;
                    errorcount++;

                    if (errorcount > 1000)
                    {
                        return;
                    }
                }

                levelManager.occupied[vi] = true;

                Vector3 pos = new Vector3((float)x, y, (float)z);

                y = terrain.getHeightAt(pos) + 2.0f;

                pos.Set(x, y, z);

                child.transform.position = pos;

                //control the spread <JK> final analysis, full spread is better
                /*
                min-= 5;
                max+= 5;

                if (min < 1)
                {
                    min = 1;
                }
                if (max > terrain.xSize)
                {
                    max = terrain.xSize;
                }
                */

                treeCount++;
            }
        }
    }




}