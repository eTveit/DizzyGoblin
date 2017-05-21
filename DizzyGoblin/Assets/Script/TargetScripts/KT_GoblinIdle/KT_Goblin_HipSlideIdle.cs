//copyright Kitty Toft 2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KT_Goblin_HipSlideIdle : MonoBehaviour
{

    public float speed = 1;
    public float range = 0.25f;
    public float phase = 0;
    public float yOffset = 0;
    public float speedMod = 0.2f;

    private float initXpos;

    public Transform AvatarObj;
    private GoblinGlobals goblinGlobals;


    // Use this for initialization
    void Start()
    {
        goblinGlobals = AvatarObj.GetComponent<GoblinGlobals>();
        initXpos = transform.localPosition.z;

    }

    // Update is called once per frame
    void Update()
    {

        speed = goblinGlobals.speed * speedMod;

        Vector3 lpos = transform.localPosition;
        lpos.Set(initXpos + Mathf.Sin((Time.time * speed) + phase) * range, lpos.y + yOffset, lpos.z);

        transform.localPosition = lpos;


        //do we need to check y on the hips?
        //Vector3 pos = transform.position;
        //float y = mesh.getHeightAt(pos);
        //pos.y = y + 0.2f;

        //transform.position = pos;


    }

}
