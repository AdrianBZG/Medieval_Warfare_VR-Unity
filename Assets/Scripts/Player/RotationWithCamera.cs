using UnityEngine;
using System.Collections;

public class RotationWithCamera : MonoBehaviour {

	public Transform cameraTrans;
	public float camHeightOffset;
	// Use this for initialization

	private float cameraHeight;
	private Quaternion rot;


	public float initialDistance;

	void Start () {
		Vector3 cameraPos = new Vector3 (cameraTrans.position.x, 0.0f, cameraTrans.position.z);
		Vector3 myPos = new Vector3 (transform.position.x, 0.0f, transform.position.z);
		cameraHeight = cameraTrans.position.y;
		initialDistance = Vector3.Distance (cameraPos, myPos);
	}
		
	
	void LateUpdate () {
		
		cameraTrans.position = new Vector3(transform.position.x + transform.forward.x * initialDistance, 
																	 transform.position.y + camHeightOffset, 
																	 transform.position.z + transform.forward.z * initialDistance);

		rot = new Quaternion(0,
											 cameraTrans.rotation.y,
											 0,
											 cameraTrans.rotation.w);
		transform.rotation = rot;
	}
}
