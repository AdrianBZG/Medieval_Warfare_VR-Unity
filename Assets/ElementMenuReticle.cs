using UnityEngine;
using System.Collections;
using System;

public class ElementMenuReticle : MonoBehaviour, IGvrGazeResponder {


    public Vector3 originalScale;
    public void OnGazeEnter()
    {
        transform.localScale = new Vector3(1.5f, 0.25f, 1.5f);
        GetComponent<Renderer>().material.color = Color.blue;
    }

    public void OnGazeExit()
    {
        transform.localScale = originalScale;
        GetComponent<Renderer>().material.color = Color.white;
    }

    public void OnGazeTrigger()
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {
        originalScale = transform.localScale;
	}

}
