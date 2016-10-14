using UnityEngine;
using System.Collections;

public class VRRotationManager : MonoBehaviour {

    public Transform head;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(head.rotation.ToEulerAngles());
	}
}
