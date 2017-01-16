using UnityEngine;
using System.Collections;

public class keyGoldManager : MonoBehaviour {

    public float rotSpeed = 3.0f;

	
	void Update () {
        transform.Rotate(Vector3.up, rotSpeed);
	}

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player")
        {
            if (GameManager.allEnemiesDead())
            {
                GameManager.setPlayerHaveKey(true);
                GetComponent<GvrAudioSource>().Play();
                Invoke("makeInvisible", 0.25f);

            }
        }
    }

    void makeInvisible ()
    {
        gameObject.SetActive(false);
    }
}
