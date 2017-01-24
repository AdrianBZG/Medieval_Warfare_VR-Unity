using UnityEngine;
using System.Collections;
using System;

public class MenuOnGazeManager : MonoBehaviour, IGvrGazeResponder {

    private bool onContinueButton = false;
    private bool onDevelopersButton = false;

    public void OnGazeEnter()
    {
        if (gameObject.name == "ContinueButton")
        {
            onContinueButton = true;
            print("OnContinueButton");
        }
        else if (gameObject.name == "DevelopersButton")
        {
            onDevelopersButton = true;
            print("OnDevelopersButton");
        }
    }

    public void OnGazeExit()
    {
        onContinueButton = false;
    }

    public void OnGazeTrigger()
    {
       
    }

}
