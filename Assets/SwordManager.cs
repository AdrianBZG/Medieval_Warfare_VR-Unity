using UnityEngine;
using System.Collections;

public class SwordManager : MonoBehaviour {

    public GvrAudioSource attackSound;
    public PlayerManager playerManager = null;
    public int swordDamage = 20;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Obstacle")
            attackSound.Play();
        if ((tag == "Player" && col.tag == "Enemy") ||(tag == "Ally" && col.tag == "Enemy"))
        {
            bool wantAttack = true;
            if (playerManager != null)
            {
                wantAttack = playerManager.getWantAttack();
            }

            if (!col.gameObject.GetComponent<AIAgent>().isDead() && wantAttack)
            {
                attackSound.Play();
                col.gameObject.GetComponent<AIAgent>().getDamage(swordDamage);
            }
        }
    }
}
