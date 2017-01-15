using UnityEngine;
public class GameManager : MonoBehaviour {

    public delegate void MyDelegate();
    public static MyDelegate myDelegate;


    public MenuManager menuManager;
    
	// public GameObjects
	public GameObject player;
	public GameObject campFire;

    private static int numEnemiesAlive;
    private static int numAlliesAlive;
    
    private static int points;

    private static bool playerHaveKey = false;




    private Camera[] cams;
    private Vector3[] originalCamPositions;
    private static float shake = 0;
    private static float shakeAmount = 0.7f;
    private static float decreaseFactor = 1.0f; 


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



        cams = GameObject.FindObjectsOfType<Camera>();
        originalCamPositions = new Vector3[cams.Length];

        originalCamPositions[0] = cams[0].transform.localPosition;
        originalCamPositions[1] = cams[1].transform.localPosition;

    }



    // This function must be called every time an enemy is killed.
    public static void KilledEnemy (int p)
    {
        points += p;
        numEnemiesAlive--;
        //CheckWin();
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

    public static bool allEnemiesDead ()
    {
        return numEnemiesAlive == 0;
    }

    public static void setPlayerHaveKey (bool haveKey)
    {
        playerHaveKey = haveKey;
        print("PlayerHaveKey");
    }

    public static bool getPlayerHaveKey ()
    {
        return playerHaveKey;
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

        if (shake > 0)
        {
            foreach (Camera cam in cams)
            {
                cam.transform.localPosition = Random.insideUnitSphere * shakeAmount;
                shake -= Time.deltaTime * decreaseFactor;
            }
        }
        else
        {
            shake = 0.0f;
            cams[0].transform.localPosition = originalCamPositions[0];
            cams[1].transform.localPosition = originalCamPositions[1];
        }
    }
    
    public void ShowMenu ()
    {
        if (!menuManager.IsActive())
        {
            PauseGame();
            menuManager.Show();
        }
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
        menuManager.Hide();
        print("restore game");
        foreach (GvrAudioSource source in GameObject.FindObjectsOfType<GvrAudioSource>())
        {
            source.UnPause();
            print("source unpaused");
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

    public static void ShakeScreen ()
    {
        shake = 1.5f;
    }


}
