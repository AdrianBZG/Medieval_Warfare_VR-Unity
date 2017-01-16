using UnityEngine;
using System.Collections;

public class TrollSoundManager : MonoBehaviour {

    public GvrAudioSource walkingSound;
    public GvrAudioSource damageSound;
    public GvrAudioSource deadSound;
    public GvrAudioSource HammerSound;


    private bool isWalking = false;
    
    public bool IsWalking()
    {
        return isWalking;
    }

    public void Walk ()
    {
        if (!isWalking)
        {
            isWalking = true;
            walkingSound.Play();
        }
    }


    public void StopWalking()
    {
        isWalking = false;
        walkingSound.Stop();
    }

    public void Damage ()
    {
        damageSound.Play();
    }

    public void Dead ()
    {
        deadSound.Play();
    }

    public void Sword ()
    {
        HammerSound.Play();
    }


}
