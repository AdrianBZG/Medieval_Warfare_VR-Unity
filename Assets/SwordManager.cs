using UnityEngine;
using System.Collections;

public class SwordManager : MonoBehaviour {

    public GvrAudioSource attackSound;
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
            attackSound.Play();
    }
}
