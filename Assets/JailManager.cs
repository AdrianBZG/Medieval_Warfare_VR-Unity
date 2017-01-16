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
            }
            kidnaped.GetComponent<GvrAudioSource>().Play();
            kidnaped.GetComponent<KidnapedManager>().Liberate();

        }
        else if (col.tag == "Player")
        {
            GetComponent<GvrAudioSource>().Play();
        }
    }
}
