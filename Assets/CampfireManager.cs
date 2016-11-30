using UnityEngine;
using System.Collections;

public class CampfireManager : MonoBehaviour {

	public Light fireLight;
	public ParticleSystem fireParticles;
	public ParticleSystem sparkParticles;

	private bool isOn;

	// Use this for initialization
	void Start () {
		isOn = true;
	
	}


	public void Switch () {
		if (IsOn()) 
			TurnOff();
		else
			TurnOn();
	}

	public void TurnOn () {
		SetTurn(true);
	}

	public void TurnOff () {
		SetTurn(false);
	}

	private void SetTurn (bool turningOn) {
		isOn = turningOn;
		fireParticles.gameObject.SetActive(turningOn);
		sparkParticles.gameObject.SetActive(turningOn);
		fireLight.gameObject.SetActive(turningOn);
	}

	public bool IsOn () {
		return isOn;
	}



}
