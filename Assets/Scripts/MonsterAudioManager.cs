using UnityEngine;
using System.Collections;

public class MonsterAudioManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventsManager.onAreaReached += PlaySound;
	}

    // Update is called once per frame
    public void PlaySound()
    {
        GetComponent<AudioSource>().Play();
    }
}
