using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    private bool wantAttack;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool getWantAttack (){
        return wantAttack;
    }

    public void WantAttack ()
    {
        wantAttack = true;
    }

    public void DontWantAttack ()
    {
        wantAttack = false;
    }


}
