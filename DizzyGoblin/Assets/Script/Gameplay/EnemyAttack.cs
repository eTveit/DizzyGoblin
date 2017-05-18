using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    public int DamageToPlayer = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            //other.gameObject.GetComponent<HealthSystem>().DamagePlayer(DamageToPlayer);
			Debug.Log ("HIT");
        }
    }

}
