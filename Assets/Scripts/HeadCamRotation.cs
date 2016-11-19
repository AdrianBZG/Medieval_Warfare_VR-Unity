using UnityEngine;
using System.Collections;

public class HeadCamRotation : MonoBehaviour {


    public Transform camera;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        
        transform.localRotation = Quaternion.Euler(-camera.localRotation.eulerAngles.y,
                                                          camera.localRotation.eulerAngles.z ,
                                                         -camera.localRotation.eulerAngles.x );// + rotationOffset;

        

    }
}
