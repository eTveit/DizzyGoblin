//Made by Lars Joar Bjørkeland

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJB_levelManager : MonoBehaviour {

    public MakeTrees makeTrees;
    public MakeFence makeFence;
    public MakeRocks makeRocks;
    public Transform ratHole;
    public TerrainMesh terrain;

    public bool[] occupied;

    public int levelDifficulty = 1;
    public int Trees = 0;
    public int Rocks = 0;
    public int Enemies = 0;

    void Start()
    {


    }

    void Update()
    {

        //<JK>  don't think this should be here, we set this when we build
        //      the level below, so this would clobber those values, every frame
        /*
         Trees = levelDifficulty * 2;
         Rocks = levelDifficulty * 2;
         Enemies = levelDifficulty * 2;
        */

        if (Input.GetKeyUp(KeyCode.E))
        {
            Win();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            Lose();
        }


    }

    void Win()
    {
        levelDifficulty++;
        if (levelDifficulty > 6)    //60 trees
            levelDifficulty = 6;

        BuildLevel();
    }
    void Lose()
    {
        //optional reset or minus one, or something...
        levelDifficulty--;
        if (levelDifficulty < 1)
            levelDifficulty = 1;

        BuildLevel();
    }

    void BuildLevel()
    {

        terrain.Generate(levelDifficulty);

        occupied = new bool[terrain.mesh.vertexCount];

        Trees = 10 * levelDifficulty;
        makeTrees.BuildTrees(Trees);

        Rocks = 10 * levelDifficulty;
        makeRocks.BuildRocks(Rocks);

        makeFence.BuildFence();

        placeRatHole();

        //BuildEnemies;

    }

    void placeRatHole()
    {


        int x, z;
        float y;

        x = Random.Range(terrain.xSize/2, terrain.xSize - 5);
        z = Random.Range(terrain.zSize/2,terrain.xSize - 5);
        y = 0;



        int vi = terrain.getVertexIndexFromXZ(x, z); // z * (terrain.xSize + 1) + x;


        int errorcount = 0;
        while (occupied[vi])
        {

            Debug.Log("try again");

            x = Random.Range(terrain.xSize / 2, terrain.xSize - 5);
            z = Random.Range(terrain.zSize / 2, terrain.xSize - 5);
            y = 0;

            //check if this point is occupied
            vi = terrain.getVertexIndexFromXZ(x, z); // z * (terrain.xSize + 1) + x;
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

        ratHole.position = pos;

    }

}

