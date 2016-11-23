using UnityEngine;
using System.Collections;

public class UIShowInput : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		int e = System.Enum.GetNames(typeof(KeyCode)).Length;
		for (int i = 0; i < e; i++)
		{
			if (Input.GetKey((KeyCode)i))
			{
				GetComponent<TextMesh>().text = ((KeyCode)i).ToString();
			}
		}
	}
}
