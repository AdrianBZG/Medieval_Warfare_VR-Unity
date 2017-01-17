using UnityEngine;
using System.Collections;

public class TrollSpawnManager : MonoBehaviour {

    public GameObject trollPrefab;
    public GameObject recruitmentText;
    public float spawnInterval;
    private static bool keyCaptured = false;

    private bool waitingForRecruitment;

    // Use this for initialization
    void Start () {
        keyCaptured = false;
        waitingForRecruitment = false;
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

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            if (!IsWaitingForRecruitment())
                ActivateRecruitmentText();
            recruitmentText.transform.LookAt(GameObject.Find("Player").transform.position);
        }
    }

    void OnTriggerExit (Collider col)
    {
        if (col.tag == "Player")
        {
            if (IsWaitingForRecruitment())
                DeactivateRecruitmentText();

        }
    }

    public void ActivateRecruitmentText ()
    {
        recruitmentText.SetActive(true);
        waitingForRecruitment = true;
    }

    public bool IsWaitingForRecruitment ()
    {
        return waitingForRecruitment;
    }

    public void DeactivateRecruitmentText()
    {
        recruitmentText.SetActive(false);
    }
}
