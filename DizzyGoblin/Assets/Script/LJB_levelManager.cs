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
    public PoolSystem poolSystem;

    //<JK> changed to integer so I can add rats as temporary "occupiers" to optimize collision avoidance.
    //     crazy optimize btw... ;)
    public int[] occupied;

    public int levelDifficulty = 1;
    public int Trees = 0;
    public int Rocks = 0;
    public int Rats = 0;

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

        //Stop objects from spawning on top of each other, also use for collision avoidance
        occupied = new int[terrain.mesh.vertexCount];

        //Trees
        Trees = 10 * levelDifficulty;
        makeTrees.BuildTrees(Trees);

        //Rocks
        Rocks = 10 * levelDifficulty;
        makeRocks.BuildRocks(Rocks);

        //Fence
        makeFence.BuildFence();

        //EnemySpawn
        placeRatHole();

        //Enemies
        Rats = 10 * levelDifficulty;
        poolSystem.spawnRats(Rats);


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
        while (occupied[vi] > 0)
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

        //expand the occupied area to encompass a larger footprint
        //occupied now keeps track of rats and goblin too, at run-time
        //so we can use occupied to optimize collision avoidance
        occupyArea(x, z);
        

        Vector3 pos = new Vector3((float)x, y, (float)z);

        y = terrain.getHeightAt(pos) + 2.0f;

        pos.Set(x, y, z);

        ratHole.position = pos;

    }

    public void occupyArea(int x, int z)
    {
        int vi = terrain.getVertexIndexFromXZ(x, z);
        occupied[vi] = 1;

        //expand area
        occupied[terrain.getVertexIndexFromXZ(x + 1, z)] = 1;
        occupied[terrain.getVertexIndexFromXZ(x - 1, z)] = 1;
        occupied[terrain.getVertexIndexFromXZ(x + 1, z + 1)] = 1;
        occupied[terrain.getVertexIndexFromXZ(x - 1, z + 1)] = 1;
        occupied[terrain.getVertexIndexFromXZ(x + 1, z - 1)] = 1;
        occupied[terrain.getVertexIndexFromXZ(x - 1, z - 1)] = 1;
        occupied[terrain.getVertexIndexFromXZ(x, z - 1)] = 1;
        occupied[terrain.getVertexIndexFromXZ(x, z + 1)] = 1;




    }
}

