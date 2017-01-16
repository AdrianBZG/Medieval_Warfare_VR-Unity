using UnityEngine;
using System.Collections;

public class WinningManager : MonoBehaviour {

    public GameObject winObject;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public  void ShowUI()
    {
        winObject.SetActive(true);
        winObject.transform.position = transform.position;
        winObject.transform.LookAt(GameObject.Find("Player").transform.position);
        winObject.transform.Rotate(Vector3.up * 180);
    }
}
