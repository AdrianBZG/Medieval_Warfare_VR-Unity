using UnityEngine;
using System.Collections;


    public class PlayerAudioManager : MonoBehaviour
    {
    
        public GvrAudioSource walkAudio;
        public GvrAudioSource runAudio;
        public GvrAudioSource swordAudio;
        // Use this for initialization
        void Start()
        {
        }

        public void Walk()
        {

            walkAudio.Play();
        }

        public void Run()
        {
            runAudio.Play();
        }

        public void Sword()
        {
            swordAudio.Play();
        }

        public void StopWalking()
        {
            walkAudio.Stop();
        }

        public void StopRunning()
        {
            runAudio.Play();
        }

        public void StopSword()
        {
            swordAudio.Stop();
        }
    }
