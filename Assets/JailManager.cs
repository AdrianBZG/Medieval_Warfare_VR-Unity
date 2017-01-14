using UnityEngine;
using System.Collections;

public class JailManager : MonoBehaviour {

    public GameObject[] jailWalls;
    public GameObject kidnaped;

    void OnTriggerEnter (Collider col)
    {
        if (GameManager.getPlayerHaveKey() && col.tag == "Player")
        {
            foreach (GameObject wall in jailWalls)
            {
                wall.SetActive(false);
                Destroy(wall.gameObject);
            }
            kidnaped.GetComponent<GvrAudioSource>().Play();

        }
        else if (col.tag == "Player")
        {
            GetComponent<GvrAudioSource>().Play();
        }
    }
}
