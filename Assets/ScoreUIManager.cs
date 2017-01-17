using UnityEngine;
using System.Collections;

public class ScoreUIManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetScore (int points) {
        GetComponent<TextMesh>().text = "Score: " + points.ToString();
    }
    
}
