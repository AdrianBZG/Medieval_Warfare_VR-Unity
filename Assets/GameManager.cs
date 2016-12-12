using UnityEngine;
public class GameManager : MonoBehaviour {

    public delegate void MyDelegate();
    public static MyDelegate myDelegate;


    public GameObject menu;
    
	// public GameObjects
	public GameObject player;
	public GameObject campFire;

    private static int numEnemiesAlive;
    private static int numAlliesAlive;
    
    private static int points;


	// public values for comparisons bassically
    // this variable contains the maximum distance where the player can interact with the campFire 
	public float campDistEvent;


	// Private assets
	private CampfireManager campfireManager;

	// Use this for initialization
	void Start () {
		campfireManager = campFire.GetComponent<CampfireManager>();

        numEnemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        print(numEnemiesAlive + " enemies alive");
        numAlliesAlive = GameObject.FindGameObjectsWithTag("Ally").Length;
        print(numAlliesAlive + " allies alive");

    }

    // This function must be called every time an enemy is killed.
    public static void KilledEnemy (int p)
    {
        points += p;
        numEnemiesAlive--;
        CheckWin();
    }

    // This function must be called every time an ally is killed.
    public static void KilledAlly (int p)
    {
        points -= p;
        numAlliesAlive--;
        CheckLose();
    }

    private static void CheckWin ()
    {
        if (numEnemiesAlive == 0)
        {
            WinGame();
        }
    }

    private static void CheckLose()
    {
        if (numAlliesAlive == 0)
        {
            LoseGame();
        }
    }




    // Update is called once per frame
    void Update () {
		if (Vector3.Distance(player.transform.position, campFire.transform.position) < campDistEvent) {
			if (Input.GetKeyDown(KeyCode.LeftShift)) {
				campfireManager.Switch();
			}
		}

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ShowMenu();
        }
        if (myDelegate != null)
            myDelegate();
	}
    
    public void ShowMenu ()
    {
        PauseGame();
        menu.SetActive(true);
    }

    public void PauseGame ()
    {
        Time.timeScale = 0.0f;
        foreach (GvrAudioSource source in GameObject.FindObjectsOfType<GvrAudioSource>())
        {
            source.Pause();
        }
        
    }

    public void RestoreGame ()
    {
        Time.timeScale = 1.0f;
        menu.SetActive(false);
        foreach (GvrAudioSource source in GameObject.FindObjectsOfType<GvrAudioSource>())
        {
            source.UnPause();
        }
    }

    public static void EndGame ()
    {
        print("End game");
        Application.Quit();
    }

    public static void WinGame ()
    {
        // Show winnning Cartel ?
    }

    public static void LoseGame()
    {
        // Show lose Cartel ?
    }


}
