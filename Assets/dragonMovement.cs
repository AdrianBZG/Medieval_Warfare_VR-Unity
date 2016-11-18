using UnityEngine;
using System.Collections;

public class dragonMovement : MonoBehaviour {


    public float rot;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, rot, 0));
	}
}
