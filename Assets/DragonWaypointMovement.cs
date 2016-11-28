using UnityEngine;
using System.Collections;

public class DragonWaypointMovement : MonoBehaviour {

	public GameObject Path;
	PathObjectScript pathScript;
	public GameObject targetVector;
	public int wayIndex;
	public float speed;
	private Vector3 oldPos;

	// Particle systems
	public ParticleSystem fireParticles;
	public ParticleSystem smokeParticles;
	//

	// Use this for initialization
	void Start () {
		oldPos = this.transform.position;
		wayIndex = 0;
		pathScript = Path.GetComponent<PathObjectScript>();
		targetVector = pathScript.WayPoints[wayIndex];
	}

	// Update is called once per frame
	void Update () {

		if(wayIndex <= pathScript.WayPoints.Length-1)
		{
			if(Vector3.Distance(this.transform.position, targetVector.transform.position) < 1f)
			{  
				targetVector = pathScript.WayPoints[wayIndex];
				wayIndex++;
				oldPos = this.transform.position;
			}
		}
		//var q1 = Quaternion.LookRotation(targetVector.transform.position - this.transform.position);
		//this.transform.rotation = Quaternion.Slerp(this.transform.rotation, q1, Time.deltaTime);
		this.transform.LookAt(targetVector.transform.position); // look at current target-waypoint 
		transform.Rotate(-90,0,0); // Local rotation;
		this.transform.position += ((targetVector.transform.position - oldPos) * Time.deltaTime * speed);

		if (wayIndex == pathScript.WayPoints.Length) {
			wayIndex = 0;
			fireParticles.Play();
			smokeParticles.Play();
		} else if (wayIndex == 2) {
			fireParticles.Stop();
			smokeParticles.Stop();
		}
	}
}
