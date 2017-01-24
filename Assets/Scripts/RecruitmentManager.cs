using UnityEngine;
using System.Collections;
using System;

public class RecruitmentManager : MonoBehaviour, IGvrGazeResponder {

    public GameObject soldierPrefab;
    public GameObject recruitmentText;
    public GameObject notEnoughPointsText;
    public Transform soldierSpawn;
    public int soldierPrice = 50;

    private bool waitingForRecruitment;
    // Use this for initialization
    void Start () {

        waitingForRecruitment = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public int GetSoldierPrice ()
    {
        return soldierPrice;
    }

    public void ShowNotEnoughPoints()
    {
        notEnoughPointsText.SetActive(true);
        recruitmentText.SetActive(false);
    }

    public void InstantiateSoldier ()
    { 
        if (GameManager.GetScorePoints() > soldierPrice)
        {
            Instantiate(soldierPrefab, soldierSpawn.position, Quaternion.identity);
        }
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            if (!IsWaitingForRecruitment())
                ActivateRecruitmentText();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            if (IsWaitingForRecruitment())
                DeactivateRecruitmentText();
        }
    }

    public void ActivateRecruitmentText()
    {
        recruitmentText.SetActive(true);
        notEnoughPointsText.SetActive(false);
        waitingForRecruitment = true;
    }

    public bool IsWaitingForRecruitment()
    {
        return waitingForRecruitment;
    }

    public void DeactivateRecruitmentText()
    {
        recruitmentText.SetActive(false);
        waitingForRecruitment = false;
    }

    void IGvrGazeResponder.OnGazeEnter()
    {
        if (!IsWaitingForRecruitment())
            ActivateRecruitmentText();
    }

    void IGvrGazeResponder.OnGazeExit()
    {
        if (IsWaitingForRecruitment())
            DeactivateRecruitmentText();
    }

    void IGvrGazeResponder.OnGazeTrigger()
    {
    }
}
