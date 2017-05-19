using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolsystem : MonoBehaviour {
    public GameObject Rathole;
    public GameObject Pool;
    GameObject RatSelect;
    public GameObject[] Rats;
    int RatIndex;
    GameObject tpObject;
 
    

    





	// Use this for initialization
	void Start () {

       

      
        }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.G))
        {
            RatSelection();
        }

        //if (RatHp == 0) {
        //Rat.transform.position = pool.transform.position;

        //}
    }

    void RatSelection()
    {
            Rats = GameObject.FindGameObjectsWithTag("Rats");
            RatIndex = Random.Range(0, Rats.Length);
            RatSelect = Rats[RatIndex];
            tpObject = RatSelect;
            tpObject.transform.position = Rathole.transform.position;

    }

        
        
        
       
       


    }
    
      
    
    
     









