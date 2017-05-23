using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHit : MonoBehaviour {

    public bool spinningLeft = false;

    public RootState rootState = null;

    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.tag == "Tree") {
            spinningLeft = !spinningLeft;
            Debug.Log(spinningLeft);
        }
    }
}
