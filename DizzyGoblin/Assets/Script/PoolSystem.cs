﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSystem : MonoBehaviour {

    private LJB_levelManager levelManager = null;
    public GameObject ratHole;
    GameObject selectedRat;
    public GameObject[] rats;
    int ratIndex;
    GameObject tempObject;
    public bool[] activeRats;

    private int deadRats = 0;
    private int aliveRats = 0;
    private int maxRats = 5;
    private int ratsOnLevel = 0;


    //Experimenting


    // Use this for initialization
    void Awake() {
        levelManager = GetComponent<LJB_levelManager>();
        //Declares what gameobject is rats by tag "Rats"
        rats = GameObject.FindGameObjectsWithTag("Rats");
        //Declares new bool array based on number of "Rats" in pool
        activeRats = new bool[rats.Length];

        foreach(GameObject rat in rats) {
            rat.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update() {

        Debug.Log("Alive: " + aliveRats + ", Dead: " + deadRats);

        if(aliveRats < maxRats && deadRats + aliveRats < ratsOnLevel) {
            spawnExtraRat();
        }

        if(aliveRats <= 0 && deadRats >= ratsOnLevel) {
            levelManager.Win();
        }

    }

    public void spawnRats(int numRats) {

        Debug.Log("RATZZ" + numRats);


        ratsOnLevel = numRats;
        deadRats = 0;
        aliveRats = 0;

        for(int i = 0; i < numRats; i++) {
            RatSelection(i);
        }

        int j = 0;
        foreach(GameObject rat in rats) {
            if(activeRats[j]) {
                rat.SetActive(true);
                aliveRats++;
                if(aliveRats >= maxRats) {
                    return;
                }
            }
            j++;
        }
    }

    public void spawnExtraRat() {
        int i = 0;
        foreach(GameObject rat in rats) {
            if(activeRats[i]) {
                if(!rat.activeSelf) {
                    rat.SetActive(true);
                    aliveRats++;
                    return;
                }
            }
            i++;
        }
    }



    void RatSelection(int ratIndex) {

        activeRats[ratIndex] = true;
        selectedRat = rats[ratIndex];

        //assign an index
        selectedRat.GetComponent<ratData>().ratIndex = ratIndex;
        //RatSelect is gameobject that is to be transformed to spawnpoint
        Vector3 posOffset = new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f));
        selectedRat.transform.position = ratHole.transform.position + posOffset;
    }

    public void ReturnRatToPool(GameObject theRat) {
        Debug.Log("Returning to Rat Pool");
        int id = theRat.GetComponent<ratData>().ratIndex;
        Debug.Log("Got index");
        activeRats[id] = false;
        Debug.Log("Set inactive");

        deadRats++;
        aliveRats--;
    }






    //Ole Andreas Stavå
    //Modified by Espen Tveit
}



