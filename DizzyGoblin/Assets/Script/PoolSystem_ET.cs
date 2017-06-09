using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSystem_ET : MonoBehaviour {

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
    void Start() {
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
        
        if(aliveRats > maxRats && deadRats + aliveRats < ratsOnLevel) {
            spawnRats(1, ratsOnLevel);
        }

    }

    //Informs the game when there is no more rats to get from the pool
    public void spawnRats(int numRats, int totalRatsOnLevel) {

        ratsOnLevel = totalRatsOnLevel;

        for(int i = 0; i < numRats; i++) {
            RatSelection(i);
        }

        int j = 0;
        foreach(GameObject rat in rats) {
            if(activeRats[j]) {
                rat.SetActive(true);
                j++;
                aliveRats++;
                if(aliveRats >= maxRats) {
                    return;
                }
            }
        }
    }


    void RatSelection(int ratIndex) {
        activeRats[ratIndex] = true;
        selectedRat = rats[ratIndex];
        
        //RatSelect is gameobject that is to be transformed to spawnpoint
        Vector3 posOffset = new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f));
        selectedRat.transform.position = ratHole.transform.position + posOffset;
    }

    public void returnRatToPool(GameObject theRat) {

        int id = theRat.GetComponent<ratData>().ratIndex;
        activeRats[id] = false;

    }






    //Ole Andreas Stavå
    //Modified by Espen Tveit
}














