using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class targetMoveChain : targetMove {


    private charState AvatarState;
    private Vector3 initialPos = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start() {
        AvatarState = AvatarObj.GetComponent<charState>();
        initialPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update() {
        if(AvatarState.isSpinning) {
            MoveChain(initialPos);
        }
        else {
            //This math needs to be fixed!
            Vector3 groundPos = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
            float yDiff = mesh.getHeightAt(transform.position) + transform.position.y;
            groundPos.y = initialPos.y - yDiff;
            MoveChain(groundPos);
        }
    }

    void MoveChain(Vector3 targetPos) {
        print(targetPos);
        transform.localPosition = Vector3.Slerp(transform.localPosition, targetPos, Time.deltaTime * 4);
    }
}