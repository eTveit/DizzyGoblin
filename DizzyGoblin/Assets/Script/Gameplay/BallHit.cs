using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHit : MonoBehaviour {

    public GoblinRootState rootState = null;

    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.tag == "Tree")
        { 
            rootState.spinstate.SwitchRotateDirection();
        }
    }
}
