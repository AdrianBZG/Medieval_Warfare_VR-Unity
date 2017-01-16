using UnityEngine;
using System.Collections;

public class KidnapedManager : MonoBehaviour {


    public GameObject kidnapedBody;
    public WinningManager winManager;
    private bool liberate = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (liberate)
        {
            kidnapedBody.transform.LookAt(GameObject.Find("Player").transform.position);
            winManager.ShowUI();
        }
	}

    public void Liberate ()
    {
        liberate = true;
    }
}
