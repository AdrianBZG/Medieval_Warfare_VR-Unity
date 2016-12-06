using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {





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
	}


    public void PlaySwordAirAttackSound()
    {

    }


}
