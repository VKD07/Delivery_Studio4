using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerFinder : MonoBehaviour
{
    AudioManager audioManager;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void PlaySound(string soundName)
    {
        audioManager.PlaySound(soundName);
    }
}
