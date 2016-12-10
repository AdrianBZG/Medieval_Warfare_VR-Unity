using UnityEngine;
using System.Collections;

public class SwordManager : MonoBehaviour {

    public GvrAudioSource attackSound;
    public int swordDamage = 20;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter (Collider col)
    {
        print(col.tag);
        if (col.tag == "Enemy" || col.tag == "Obstacle")
        {
            attackSound.Play();
            if (col.tag == "Enemy")
            {
                col.gameObject.GetComponent<AIAgent>().getDamage(swordDamage);
            }
        }
    }
}
