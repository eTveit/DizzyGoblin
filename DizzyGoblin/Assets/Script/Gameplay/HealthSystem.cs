using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {

    GoblinGlobals GoblinGlobals;
    public int playerMaxHealth = 5;
    public int playerCurrentHealth = 5;
    float Timer = 0.0f;
    public float Invincibility = 2.0f;

    //Heart UI
    public GameObject Heart1;
    public GameObject Heart2;
    public GameObject Heart3;
    public GameObject Heart4;
    public GameObject Heart5;


    // Use this for initialization
    void Start () {
        playerCurrentHealth = playerMaxHealth;
        GoblinGlobals = GetComponent<GoblinGlobals>();
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Timer += Time.deltaTime;

        Health();
	}



    public void DamagePlayer(int damage)
    {
        if (Timer > Invincibility)
        { 
            playerCurrentHealth -= damage;
            Timer = 0;

            Debug.Log("HIT");

        }
    }

    public void SetMaxHealth()
    {
        playerCurrentHealth = playerMaxHealth;
    }

    void Health()
    {
        if (playerCurrentHealth == 5)
        {
            Heart5.SetActive(true);
            Heart4.SetActive(true);
            Heart3.SetActive(true);
            Heart2.SetActive(true);
            Heart1.SetActive(true);
        }

        if (playerCurrentHealth == 4)
        {
            Heart5.SetActive(false);
            Heart4.SetActive(true);
        }

        if (playerCurrentHealth == 3)
        {
            Heart4.SetActive(false);
            Heart3.SetActive(true);
        }

        if (playerCurrentHealth == 2)
        {
            Heart3.SetActive(false);
            Heart2.SetActive(true);
        }

        if (playerCurrentHealth == 1)
        {
            Heart2.SetActive(false);
            Heart1.SetActive(true);
        }

        if (playerCurrentHealth <= 0)
        {
            Heart1.SetActive(false);
            GoblinGlobals.enabled = false;
        }
    }

}
//Thieu Phong Le