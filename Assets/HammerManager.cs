using UnityEngine;
using System.Collections;

public class HammerManager : MonoBehaviour {

    public TrollSoundManager soundManager;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter (Collider col)
    {
        if (col.tag == "Player" /*&& !col.gameObject.GetComponent<AIAgent>().isDead() */)
        {
            soundManager.Sword();
        }
    }
}
