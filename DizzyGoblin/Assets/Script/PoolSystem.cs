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


    //Experimenting

    
   

   

   

    // Use this for initialization
	void Start ()
    {
        //Declares what gameobject is rats by tag "Rats"
        Rats = GameObject.FindGameObjectsWithTag("Rats");
        //Declares new bool array based on number of "Rats" in pool
        ratSelected = new bool[Rats.Length];

        

    }
	
	// Update is called once per frame
	void Update () {
        
        if(Input.GetKeyDown(KeyCode.G))
        {
            if (RatSelection() == false)
                Debug.Log("no free rats");
        }


    }

    //Informs the game when there is no more rats to get from the pool
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
        //RatIndex selects random gameobject within pool    
        RatIndex = Random.Range(0, Rats.Length);

        int errorCount = 0;
        //A loop that checks if there is any rats left and stops when its empty
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
        //RatSelect is gameobject that is to be transformed to spawnpoint
        Vector3 posOffset = new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f));
        tpObject = RatSelect;
        tpObject.transform.position = Rathole.transform.position + posOffset;

        

        

        



            
       

        return true;
    }

    public void returnRatToPool(GameObject theRat)
    {

        int id = theRat.GetComponent<ratData>().ratIndex ;
        ratSelected[id] = false;

    }

    
    
       
        

    //Ole Andreas Stavå

}
    
      
    
    
     









