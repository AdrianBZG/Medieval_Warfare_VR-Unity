using UnityEngine;
using System.Collections;

public class UpdatePlayerRotationFromView : MonoBehaviour {

	public GameObject playerController;

	// Update is called once per frame
	void Update () {
		Quaternion playerRotation = playerController.transform.rotation;
		playerRotation.y = this.transform.rotation.y;
		playerController.transform.rotation = playerRotation;
	}
}
