//Made by Lars Joar Bjørkeland

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJB_levelManager : MonoBehaviour {

    public MakeTrees makeTrees;

    public int levelDifficulty = 0;
    public int Trees = 0;
    public int Rocks = 0;
    public int Enemies = 0;

    void Start()
    {

        levelDifficulty = 0;

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
        BuildLevel();
    }
    void Lose()
    {
        levelDifficulty = 0;
        BuildLevel();
    }

    void BuildLevel()
    {
        //BuildTrees;
        //BuildRocks;
        //BuildEnemies;

    }
}

