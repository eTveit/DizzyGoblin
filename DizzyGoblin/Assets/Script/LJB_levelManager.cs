//Made by Lars Joar Bjørkeland

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJB_levelManager : MonoBehaviour {

    public MakeTrees makeTrees;
    public MakeFence makeFence;
    public int something = 1;

    public int levelDifficulty = 1;
    public int Trees = 0;
    public int Rocks = 0;
    public int Enemies = 0;

    void Start()
    {


    }

    void Update()
    {

        Trees = levelDifficulty * 2;
        Rocks = levelDifficulty * 2;
        Enemies = levelDifficulty * 2;

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
        Trees = 10 * levelDifficulty;
        makeTrees.BuildTrees(Trees);
        
        //BuildTrees;
        //BuildRocks;
        //BuildEnemies;

    }
}

