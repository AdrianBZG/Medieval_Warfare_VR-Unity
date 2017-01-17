using UnityEngine;
using System.Collections;

public class RotateToPlayer : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(GameObject.Find("Player").transform.position);
	}
}
