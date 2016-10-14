using UnityEngine;
using System.Collections;

public class VRMovementManager : MonoBehaviour {


    public CharacterController controller;
    public ControllerManager mainPlayer;
    //private CharacterController controller;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        
        if (Input.GetButton("Fire1")) {
            Walk();
        }
	
	}

    void Walk ()
    {
        print("Walk");
           // mainPlayer.Move(transform.forward * walkSpeed * Time.fixedDeltaTime);
    }
}
