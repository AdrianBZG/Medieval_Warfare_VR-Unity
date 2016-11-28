using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuButtonManager : MonoBehaviour {

	public GameObject buttons;
	public GameObject developers;
	public GameObject manual;


	void Start() {
		developers.SetActive (false);
		manual.SetActive (false);
	}

	public void NewGame(string levelName) {
		SceneManager.LoadScene (levelName);
	}

	public void ShowDevelopers() {
		developers.SetActive (true);
		buttons.SetActive (false);
	}

	public void ShowManual() {
		manual.SetActive (true);
		buttons.SetActive (false);
	}

	public void QuitGame() {
		Application.Quit ();
	}

	public void BackButton() {
		developers.SetActive (false);
		manual.SetActive (false);
		buttons.SetActive (true);
	}
}
