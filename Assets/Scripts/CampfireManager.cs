using UnityEngine;
using System.Collections;
using System;

public class CampfireManager : MonoBehaviour, IGvrGazeResponder {

	public Light fireLight;
	public ParticleSystem fireParticles;
	public ParticleSystem sparkParticles;

	private bool isOn;

	// Use this for initialization
	void Start () {
		isOn = true;
	
	}

    // Switch between on and off.
	public void Switch () {
		if (IsOn()) 
			TurnOff();
		else
			TurnOn();
	}

    // Turn the campfire On
	public void TurnOn () {
		Turn(true);
	}

    // Turn the campfire Off
	public void TurnOff () {
		Turn(false);
	}

    // This method allows to turn on or off the campfire.
	private void Turn (bool turningOn) {
		isOn = turningOn;
		fireParticles.gameObject.SetActive(turningOn);
		sparkParticles.gameObject.SetActive(turningOn);
		fireLight.gameObject.SetActive(turningOn);
	}

    // This method returns true if the campfire is on.
	public bool IsOn () {
		return isOn;
	}

    // When the player looks at the campfire it turns on.
    void IGvrGazeResponder.OnGazeEnter()
    {
        TurnOn();
    }

    // When the player look away the campfire turns off.
    void IGvrGazeResponder.OnGazeExit()
    {
        TurnOff();
    }

    void IGvrGazeResponder.OnGazeTrigger()
    {
        // Nothing to do
    }
}
