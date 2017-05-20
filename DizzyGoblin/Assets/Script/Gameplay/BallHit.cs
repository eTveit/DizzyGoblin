using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHit : MonoBehaviour {

    public Transform avatarObj;

    private SpinState spinstate;

    void Start() {
        //How do I access the goblin's spinstate?
        //spinstate = avatarObj...;
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Hit something");
        if(collision.collider.tag != "Enemy"){
            Debug.Log("Not enemy... SWITCH!");
            spinstate.SwitchRotateDirection();
        }
    }
}
