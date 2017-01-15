using UnityEngine;
using System.Collections;


public class PlayerAudioManager : MonoBehaviour
{
    
    public GvrAudioSource walkAudio;
    public GvrAudioSource runAudio;
    public GvrAudioSource swordAudio;
    public GvrAudioSource auchAudio;


    private bool isWalking = false;
    private bool isRunning = false;

    
    public bool IsWalking ()
    {
        return isWalking;
    }

    public bool IsRunning ()
    {
        return isRunning;
    }

    public void Walk()
    {
        if (!isWalking)
        {
            isWalking = true;
            walkAudio.Play();
        }
    }

    public void Auch ()
    {
        auchAudio.Play();
    }

    public void Run()
    {
        if (!isRunning)
        {
            isRunning = true;
            runAudio.Play();
        }
    }

    public void Sword()
    {
        swordAudio.Play();
    }

    public void StopWalking()
    {
        isWalking = false;
        walkAudio.Stop();
    }

    public void StopRunning()
    {
        isRunning = false;
        runAudio.Stop();
    }

    public void StopSword()
    {
        swordAudio.Stop();
    }
}
