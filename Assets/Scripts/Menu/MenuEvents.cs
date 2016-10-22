using UnityEngine;
using System.Collections;

public class MenuEvents : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void LoadDungeon() {
        Debug.Log ("Loading Dungeon");
    }

    public void LoadMaze() {
        Debug.Log ("Loading Maze");
    }

    public void LoadOptions() {
        Debug.Log ("Loading Options");
    }

    public void Exit() {
        Debug.Log ("Exiting of application");
    }
}
