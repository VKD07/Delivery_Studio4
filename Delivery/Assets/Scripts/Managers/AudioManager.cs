using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField] AudioSource musicSource, sfxSource;
    public List<AudioSource> spawnedSounds;

    [Header("=== AUDIO LIBRARY ===")]
    public Sound[] sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    

    public void PlaySound(string name)
    {
        try
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i].audioName == name)
                {
                    switch (sounds[i].type)
                    {
                        case AudioType.Music:
                            musicSource.clip = sounds[i].clip;
                            musicSource.Play();
                            break;
                        case AudioType.SFX:
                            sfxSource.PlayOneShot(sounds[i].clip, sounds[i].volume);
                            break;
                    }
                    break;
                }
            }
        }
        catch (System.Exception)
        {
            Debug.Log("Sound Not Found!");
        }
    }
}

[System.Serializable]
public class Sound
{
    public string audioName;
    public AudioClip clip;
    public float volume;
    public AudioType type;
}

public enum AudioType
{
    None = 0,
    SFX,
    Music,
}
