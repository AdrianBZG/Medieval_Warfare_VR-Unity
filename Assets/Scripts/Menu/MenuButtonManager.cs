using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuButtonManager : MonoBehaviour {

	public void NewGame(string levelName) {
		SceneManager.LoadScene (levelName);
	}

	public void ShowDevelopers() {
		//
	}

	public void ShowManual() {
		//
	}

	public void QuitGame() {
		Application.Quit ();
	}
}
