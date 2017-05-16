using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSystem : MonoBehaviour {
    public GameObject SpawnPoint1;
    public GameObject SpawnPoint2;
    public GameObject targetTp;



    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            targetTp.transform.position = SpawnPoint1.transform.position;



        }

    }

    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Rats" && Input.GetKeyDown(KeyCode.Mouse1))
        {
            targetTp.transform.position = SpawnPoint2.transform.position;
        }

    }




}



