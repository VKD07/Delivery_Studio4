using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource engineSource;
    
    public void SetEngineSound(float volume, float pitch)
    {
        engineSource.pitch = pitch;
        Debug.Log(pitch);
    }
}
