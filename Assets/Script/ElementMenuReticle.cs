using UnityEngine;
using System.Collections;
using System;

public class ElementMenuReticle : MonoBehaviour, IGvrGazeResponder {

	public Color highlight = Color.cyan;
	public Color normal = Color.white;

	private Vector3 originalScale;
	private bool listenClick = false;

    public void OnGazeEnter()
    {
		transform.localScale = new Vector3(1.5F, 0.3F, 0.2F);
		GetComponent<Renderer> ().material.color = highlight;
		listenClick = true;
    }

    public void OnGazeExit()
    {
		transform.localScale = originalScale;
		GetComponent<Renderer> ().material.color = normal;
		listenClick = false;
    }

    public void OnGazeTrigger()
    {
        throw new NotImplementedException();
    }

    void Start () {
        originalScale = transform.localScale;
	}

	void Update () {
		// se puede probar esto o directamente con el sistema de eventos Event Triger component(el caso es si funciona con el mando)
		if (listenClick && Input.GetMouseButton (0)) { // TODO: Cual es el input del Mando??
			// 
			Debug.Log("Button Action");
		}
	}
}
