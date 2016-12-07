using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
public class GameManager : MonoBehaviour {



    public GameObject menu;

    public GvrAudioListener listener;


	// public GameObjects
	public GameObject player;
	public GameObject campFire;


	// public values for comparisons bassically
	public float campFireDistanceOffset;


	// Private assets
	private CampfireManager campfireManager;




	// Use this for initialization
	void Start () {
		campfireManager = campFire.GetComponent<CampfireManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(player.transform.position, campFire.transform.position) < campFireDistanceOffset) {
			if (Input.GetKeyDown(KeyCode.LeftShift)) {
				campfireManager.Switch();
			}
		}

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ShowMenu();
        }
	}

    
    public void ShowMenu ()
    {
        PauseGame();
        menu.SetActive(true);
    }

    public void PauseGame ()
    {
        Time.timeScale = 0.0f;
        foreach (GvrAudioSource source in GameObject.FindObjectsOfType<GvrAudioSource>())
        {
            source.Pause();
        }
        
    }

    public void RestoreGame ()
    {
        Time.timeScale = 1.0f;
        menu.SetActive(false);
        foreach (GvrAudioSource source in GameObject.FindObjectsOfType<GvrAudioSource>())
        {
            source.UnPause();
        }
    }


}
