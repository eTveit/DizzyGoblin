﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class targetMoveSpin : targetMove {



    private charState AvatarState;
    //for circular movement
    public float circularHeight = -1;

    // Use this for initialization
    void Start() {
        AvatarState = AvatarObj.GetComponent<charState>();
    }

    // Update is called once per frame
    void Update() {
        speed = AvatarState.speed;

        float ypos = -666;


        //to keep our targets in line with the hips, we simply want to
        //oscillate on z axis in the LOCAL space

        Vector3 lpos = transform.localPosition;
        lpos.Set(lpos.x, lpos.y, Mathf.Sin((Time.time * speed) + phase) * range);

        if(circularHeight > 0) {
            ypos = Mathf.Cos((Time.time * speed) + phase + 3.141593f) * circularHeight;
            ypos = -ypos;
            lpos.y = ypos;
        }


        //set the local
        transform.localPosition = lpos;

        //get the global, keep the target on the terrain surface
        Vector3 pos = transform.position;
        float y = mesh.getHeightAt(pos);
        pos.y = y + 0.2f;

        if(ypos > 0) {
            pos.y += ypos;
        }

        //set the final position
        transform.position = pos;
    }
}