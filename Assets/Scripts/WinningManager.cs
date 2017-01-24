using UnityEngine;
using System.Collections;

public class WinningManager : MonoBehaviour {

    public GameObject winObject;


    public void ShowUI()
    {
        print("Show winning ui");
        winObject.SetActive(true);
        winObject.transform.position = transform.position;
        winObject.transform.LookAt(GameObject.Find("Player").transform.position);
        winObject.transform.Rotate(Vector3.up * 180);
    }
}
