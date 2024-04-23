using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource engineSource, carScreeching, carnHornSource;

    private void Update()
    {
      
    }

    #region Network Receivers
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

    public void SetEngineSound(float pitch)
    {
        engineSource.pitch = pitch;
    }

    public void PlayCarHorn()
    {
        carnHornSource.Stop();
        carnHornSource.Play();
    }

    #endregion
}
