using UnityEngine;
using System.Collections;

public class TrollSpawnManager : MonoBehaviour {

    public GameObject trollPrefab;
    public float spawnInterval;

    public Transform[] spawnPositions;
    private int inx = 0;


    private static bool keyCaptured = false;



    // Use this for initialization
    void Start () {
        keyCaptured = false;
        InvokeRepeating("SpawnTroll", 0.0f, spawnInterval);
    }

    Vector3 GetNextPosition ()
    {
        if (inx == spawnPositions.Length) inx = 0;
        return spawnPositions[inx++ % spawnPositions.Length].position;
    }

    void SpawnTroll ()
    {
        if (!GameManager.getPlayerHaveKey())
            Instantiate(trollPrefab, GetNextPosition(), Quaternion.identity);
    }

    public static void KeyCaptured ()
    {
        keyCaptured = true;
    }

}
