using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSystem : MonoBehaviour
{

    public GameObject Rathole;
    GameObject RatSelect;
    public GameObject[] Rats;
    int RatIndex;
    GameObject tpObject;
    public bool[] ratSelected;
 
    

    





	// Use this for initialization
	void Start () {
        Rats = GameObject.FindGameObjectsWithTag("Rats");
        ratSelected = new bool[Rats.Length];


    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.G))
        {
            if (RatSelection() == false)
                Debug.Log("no free rats");
        }

        //if (RatHp == 0) {
        //Rat.transform.position = pool.transform.position;

        //}
    }
    public void spawnRats(int numRats)
    {

        for (int i = 0; i < numRats; i++)
        {
            if (RatSelection() == false)
                Debug.Log("no free rats");
        }




    }
    bool RatSelection()
    {
            
        RatIndex = Random.Range(0, Rats.Length);

        int errorCount = 0;

        while (ratSelected[RatIndex] == true)
        {
            RatIndex = Random.Range(0, Rats.Length);
            errorCount++;
            if (errorCount > 1000)
                return false;
        }

        ratSelected[RatIndex] = true;
        RatSelect = Rats[RatIndex];

        //assign an index
        RatSelect.GetComponent<ratData>().ratIndex = RatIndex;

        tpObject = RatSelect;
        tpObject.transform.position = Rathole.transform.position;

        return true;
    }

    public void returnRatToPool(GameObject theRat)
    {

        int id = theRat.GetComponent<ratData>().ratIndex ;
        ratSelected[id] = false;

    }

}
    
      
    
    
     









