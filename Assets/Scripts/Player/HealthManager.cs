using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {

    public int initialHealth;

    private static int health;
	// Use this for initialization
	void Start () {
        health = initialHealth;
	}
	


    public static void Damage (int quantity)
    {
        health -= quantity;
        if (health < 0)
            health = 0;

        print("Vida del player: " + health);
        GameObject.Find("LifeBar").GetComponent<LifeBarManager>().setLifePoints(health);
        GameObject.FindObjectOfType<PlayerAudioManager>().Auch();
        CheckHealth();
    }

    public static void CheckHealth ()
    {
        if (health == 0)
        {
            GameManager.myDelegate += Dead;
        }
    }

    public static void Dead ()
    {
        GameManager.EndGame();
    }




}
