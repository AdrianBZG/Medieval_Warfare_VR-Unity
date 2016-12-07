using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {

    public int initialHealth;

    private int health;
	// Use this for initialization
	void Start () {
        health = initialHealth;
	}
	


    public void Damage (int quantity)
    {
        health -= quantity;
        if (health < 0)
            health = 0;

        CheckHealth();
    }

    void CheckHealth ()
    {
        if (health == 0)
        {
            GameManager.myDelegate += Dead;
        }
    }

    void Dead ()
    {
        GameManager.EndGame();
    }




}
