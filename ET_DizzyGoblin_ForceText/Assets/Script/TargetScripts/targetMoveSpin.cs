using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class targetMoveSpin : targetMove {
    //DONT FORGET TO NAME IT, YOUR INITIALS, AND SOME LOGICAL NAME
    //the FSM uses names of our creation to select animations to play
    //you can do it here, or in the editor. it is best to do it here
    //so when it is intantiated, it uses this name as the default
    /*
         "ET_spin";
    */

    public override string getAnimName() {
        return animationName;
    }


    //for circular movement
    public float circularHeight = -1;
    public float rotationSpeed = 100;
    public float rotationBoost = 1000;
    [Tooltip("Left foot should be set as the kicking foot.")]
    public bool isKickingFoot = false;
    public float incrementingDT = 0;

    private GoblinGlobals goblinGlobals = null;

    // Use this for initialization
    void Start() {
        goblinGlobals = AvatarObj.GetComponent<GoblinGlobals>();

        if(isKickingFoot) {
            phase = 0;
            range = 1;
            circularHeight = 1.2f;
        }
        else {
            //Why would we settle for just six decimal points?
            phase = Mathf.PI;
            range = 0.3f;
            circularHeight = -1.0f;
        }
        rotationSpeed = 100;
        rotationBoost = 100;
    }

    // Update is called once per frame
    void Update() {

        incrementingDT += Time.deltaTime;

        speed = goblinGlobals.speed;

        float ypos = -666;


        //to keep our targets in line with the hips, we simply want to
        //oscillate on z axis in the LOCAL space

        Vector3 lpos = transform.localPosition;
        lpos.Set(lpos.x, lpos.y, Mathf.Sin((incrementingDT * speed) + phase) * range);

        if(isKickingFoot) {
            lpos.x = startPosition.x;
        }
        else {
            lpos.x = startPosition.x * 0.2f;
        }

        if(circularHeight > 0) {
            ypos = Mathf.Cos((incrementingDT * speed) + phase + 3.141593f) * circularHeight;
            ypos = -ypos;
            lpos.y = ypos;
        }


        //set the local
        transform.localPosition = lpos;

        //get the global, keep the target on the terrain surface
        Vector3 pos = transform.position;
        float y = mesh.getHeightAt(pos);
        pos.y = y + heightOffset;

        if(ypos > 0) {
            pos.y += ypos;
        }

        //set the final position
        transform.position = pos;
    }
}
