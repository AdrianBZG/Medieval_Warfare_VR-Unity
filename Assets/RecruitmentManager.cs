using UnityEngine;
using System.Collections;

public class RecruitmentManager : MonoBehaviour {

    public GameObject soldatPrefab;
    public Transform soldatSpawn;
    public int soldierPrice = 50;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InstantiateSoldat ()
    { 
        if (GameManager.GetScorePoints() > soldierPrice)
        {
            Instantiate(soldatPrefab, soldatSpawn.position, Quaternion.identity);
        }
    }


    void OnTriggerEnter (Collider col)
    {

    }
}
