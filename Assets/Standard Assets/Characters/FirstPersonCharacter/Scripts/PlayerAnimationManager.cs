using UnityEngine;
using System.Collections;

public class PlayerAnimationManager : MonoBehaviour {


    public Animator playerAnim;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void WalkAnim ()
    {
        print("walk");
        playerAnim.Play("walk");
    }

    public void IdleAnim ()
    {
        print("idle");
        playerAnim.Play("idle");
    }
}
