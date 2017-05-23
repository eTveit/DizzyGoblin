//copyright Kitty Toft 2017


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KT_GoblinChain : targetMove
{

    public override string getAnimName()
    {
        return animationName;
    }

    private GoblinGlobals goblinGlobals;
    private float initialWorldY = 0;

    public bool isSpinning = false;

    // Use this for initialization
    void Start()
    {
        goblinGlobals = AvatarObj.GetComponent<GoblinGlobals>();
        initialWorldY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpinning)
        {
            MoveChain(startPosition);
        }
        else
        {
            Debug.Log("Falling to the ground");
            Vector3 groundPos = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
            float localGroundY = startPosition.y - (initialWorldY - mesh.getHeightAt(transform.position));
            groundPos.y = localGroundY;
            groundPos.z = startPosition.z / 3;
            MoveChain(groundPos);
        }
    }

    void MoveChain(Vector3 targetPos)
    {
        transform.localPosition = Vector3.Slerp(transform.localPosition, targetPos, Time.deltaTime * 4);
    }
}