using UnityEngine;
using System.Collections;

public class HammerManager : MonoBehaviour {

    public TrollSoundManager soundManager;
    public float hitInterval = 0.7f;
    private bool justHit = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerExit (Collider col)
    {
        if (col.tag == "Player" && !justHit)
        {
            justHit = true;
            soundManager.Sword();
            GameManager.ShakeScreen();
            Invoke("canHit", hitInterval);
        }

        if (col.tag == "Ally")
        {
            justHit = true;
            soundManager.Sword();
            Invoke("canHit", hitInterval);
        }
    }

    void canHit ()
    {
        justHit = false;
    }
}
