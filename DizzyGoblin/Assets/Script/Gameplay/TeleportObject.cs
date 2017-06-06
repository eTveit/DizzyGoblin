using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObject : MonoBehaviour {

    //Script should be placed on rats

    //Put the rats spawn pool in Destination
    public Transform Destination;
    float timer;
    public float DeSpawn = 3;
    bool Dead = false;
    steering steering;

    public bool isHitByBall = false;

    // Use this for initialization
    void Start() {
        steering = GetComponent<steering>();
    }

    // Update is called once per frame
    void Update() {


        timer += Time.deltaTime;
        if(Dead == true && timer > DeSpawn) {
            teleport();
            Dead = false;
            //The ideal would be to disable steering while they are in hell and enable it when they spawn. rather than enable it when they go to hell. Takes too much uneeded power.
            steering.enabled = true;
        }


    }

    public void HitByBall() {
        timer = 0;
        Dead = true;
        //Disable steering script on rat so that it stays still when dead
        steering.enabled = false;
        isHitByBall = true;
    }

    void teleport() {
        transform.position = Destination.position;
        isHitByBall = false;
    }
}
//Thieu Phong Le
