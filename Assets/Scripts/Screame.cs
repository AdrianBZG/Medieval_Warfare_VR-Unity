using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Screame : MonoBehaviour {
	public AudioSource screamS;
	public Image screamImage;
	public bool active = false;
	public Collider ScreamTrigger;
	public float Timer; 
	private float TimerDown;

	void Update () {
		if(TimerDown > 0) TimerDown -= Time.deltaTime;
		if(TimerDown < 0) TimerDown = 0;
		if(TimerDown == 0)
		{
			if(active == true)
			{
			disable ();
			}
		}

	}
	void OnTriggerEnter(Collider ScreamTrigger){
		if (active == false) {
		
			screamImage.gameObject.SetActive(true);
			screamS.Play();
			TimerDown = Timer; 
			active = true;
		} 
	}
	public void disable(){
		screamImage.gameObject.SetActive(false);

			ScreamTrigger.gameObject.SetActive (false);

	}

}
