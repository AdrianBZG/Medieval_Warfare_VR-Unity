using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(TextMesh))]
public class UIShowInput : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        int e = System.Enum.GetNames(typeof(KeyCode)).Length;
        for (int i = 0; i < e; i++)
        {
            

        }

        if (Input.anyKey)
        {
            if (Input.GetButton("Fire1"))
            {
                GetComponent<TextMesh>().text = "Fire1 button";
            }
            if (Input.GetButton("Fire2"))
            {
                GetComponent<TextMesh>().text = "Fire2 button";
            }
            if (Input.GetButton("Fire3"))
            {
                GetComponent<TextMesh>().text = "Fire3 button";
            }
            if (Input.GetButton("Fire4"))
            {
                GetComponent<TextMesh>().text = "Fire4 button";
            }

        }
        else
        {
            GetComponent<TextMesh>().text = "No button";
        }

    }
}
