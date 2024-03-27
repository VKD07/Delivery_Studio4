using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource engineSource, carScreeching;

    public void SetEngineSound(float pitch)
    {
        engineSource.pitch = pitch;
    }

    private void Update()
    {
      
    }

    public void PlayCarScreeching(bool val)
    {
        if (val)
        {
            carScreeching.Play();
        }
        else
        {
            carScreeching.Stop();
        }
    }
}
