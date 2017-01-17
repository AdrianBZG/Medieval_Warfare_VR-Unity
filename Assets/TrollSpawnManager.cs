using UnityEngine;
using System.Collections;

public class TrollSpawnManager : MonoBehaviour {

    public GameObject trollPrefab;
    public float spawnInterval;
    private static bool keyCaptured = false;



    // Use this for initialization
    void Start () {
        keyCaptured = false;
        InvokeRepeating("SpawnTroll", 0.0f, spawnInterval);
    }

    void SpawnTroll ()
    {
        if (!GameManager.getPlayerHaveKey())
            Instantiate(trollPrefab,transform.position, Quaternion.identity);
    }

    public static void KeyCaptured ()
    {
        keyCaptured = true;
    }

}
