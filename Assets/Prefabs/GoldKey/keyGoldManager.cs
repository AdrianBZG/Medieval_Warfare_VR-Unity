using UnityEngine;
using System.Collections;

public class keyGoldManager : MonoBehaviour {

    public float rotSpeed = 3.0f;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, rotSpeed);
	}

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player")
        {
            print("Player touched the key");
            if (GameManager.allEnemiesDead())
            {
                GameManager.setPlayerHaveKey(true);
                GetComponent<GvrAudioSource>().Play();
                Destroy(this.gameObject);
            }
        }
    }
}
