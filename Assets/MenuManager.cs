using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public GameManager gameManager;
    public GameObject menuObject;
    // Use this for initialization
    public GameObject menuInstance;
    public GameObject developersInstance;
    public GameObject manualInstance;
    private bool menuActive = false;
    private bool isInitialMenu = false;

    private bool menuHided = false;


    public GameObject[] buttons;
    public Color highLightedButtonColor;
    private int inxActive;       // index of the highlighted button

    public Material originalMat;
    public Material highMat;
    public Color originalColor;

    public bool IsActive ()
    {
        return menuActive;
    }

    public void IsInitialMenu ()
    {
        isInitialMenu = true;
    }
    
    
    public void Start ()
    {
        inxActive = 0;
        originalColor = originalMat.color;
    }

    public void Update ()
    {
        if (menuHided)
        {
            if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift))
            {
                foreach (GameObject button in buttons)
                {
                    button.SetActive(true);
                }
                menuHided = false;
                if (developersInstance.active)
                    developersInstance.SetActive(false);
                else if (manualInstance.active)
                    manualInstance.SetActive(false);
            }
        }
        else if (menuActive || isInitialMenu)
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                SetColorButton(inxActive, false);
                SetColorButton(GetNextButtonInx(), true);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                string buttonName = buttons[inxActive].name;
                if (buttonName == "StartButton")
                    Application.LoadLevel(1);
                if (buttonName == "ContinueButton")
                    gameManager.RestoreGame();
                else if (buttonName == "DevelopersButton")
                    ShowItem(developersInstance);
                else if (buttonName == "ManualButton")
                    ShowItem(manualInstance);
                else if (buttonName == "ExitButton")
                    Application.Quit();
            }
        }
    }

    public int GetNextButtonInx ()
    {
        if (++inxActive == buttons.Length)
            inxActive = 0;
        return inxActive;
    }
    
    
    public void Show ()
    {
        menuInstance.SetActive(true);
        menuInstance.transform.position = transform.position;
        menuInstance.transform.LookAt(GameObject.Find("Player").transform.position);
        menuInstance.transform.Rotate(Vector3.up * 180);
        SetColorButton(0, true);
        menuActive = true;
    }

    public void ShowItem (GameObject item)
    {
        item.SetActive(true);

        if (!isInitialMenu)
        {
            item.transform.position = transform.position;
            item.transform.LookAt(GameObject.Find("Player").transform.position);
            item.transform.Rotate(Vector3.up * 180);
        }
        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
        }

        menuHided = true;
    }
    


    // This method sets color (highlight or normal) for a specific button (indicated by index).
    public void SetColorButton (int index, bool highlight)
    {
        if (!highlight)
            buttons[index].GetComponent<MeshRenderer>().material = originalMat;
        else
            buttons[index].GetComponent<MeshRenderer>().material = highMat;
    }

    public void Hide ()
    {
        if (menuActive)
        {
            menuActive = false;
            menuInstance.SetActive(false);
        }
    }
}
