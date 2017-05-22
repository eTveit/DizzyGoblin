//Made by Lars Joar Bjørkeland

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeRocks : MonoBehaviour {

    public TerrainMesh terrain;
    public LJB_levelManager levelManager;

    private int rockCount = 0;

    private float rockSize = 1;

    public void BuildRocks(int num_rocks)
    {

        rockCount = 0;

        Mesh mesh = terrain.mesh;
        int vc = mesh.vertexCount;


        // occupied = new bool[vc];
        int min = 20;////Random.Range(1, terrain.xSize);
        int max = terrain.xSize - 20; // min;

        Vector3 hell = new Vector3(-666, -666, -666);

        foreach (Transform child in transform)
        {

            if (rockCount >= num_rocks)
            {
                child.transform.position = hell;
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);

                //rockSize = Random.Range(1, 100)/100.0f;               
                //transform.localScale += new Vector3(rockSize, rockSize, rockSize);
                      

                int x, z;
                float y;

                x = Random.Range(min, max);
                z = Random.Range(min, max);
                y = 0;



                int vi = z * (terrain.xSize + 1) + x;


                int errorcount = 0;
                while (levelManager.occupied[vi] > 0 || x % 2 == 0 || z % 2 == 0)
                {

                    Debug.Log("try again");

                    x = Random.Range(min, max);
                    z = Random.Range(min, max);
                    y = 0;

                    //check if this point is occupied
                    vi = z * (terrain.xSize + 1) + x;
                    errorcount++;

                    if (errorcount > 1000)
                    {
                        return;
                    }
                }

                levelManager.occupied[vi] = 1;

               //Change size of rock
                

                Vector3 pos = new Vector3((float)x, y, (float)z);

                y = terrain.getHeightAt(pos) + 0.05f;

                pos.Set(x, y, z);

                child.transform.position = pos;


                /*
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
                */

                rockCount++;
            }
        }
    }




}