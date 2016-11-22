using UnityEngine;
using System.Collections;

public class RotationWithCamera : MonoBehaviour {

	public Transform cameraTrans;
	// Use this for initialization

	public Quaternion rot;

	public Vector3 initialDistance;
	public Vector3 initialPosPlayer;

	void Start () {
		initialPosPlayer = transform.position;
		initialDistance = cameraTrans.position - transform.position;
		print(initialDistance);
	}
		
	
	void LateUpdate () {
		//rot.eulerAngles = new Vector3 (0, cameraTrans.rotation.eulerAngles.x, 0);
		cameraTrans.position = transform.position + initialDistance;

		rot = new Quaternion(0,
											 cameraTrans.rotation.y,
											 0,
											 cameraTrans.rotation.w);
		transform.rotation = rot;
	}
}
