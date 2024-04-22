using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField] AudioSource musicSource, sfxSource;

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
                int index = i;
                if (sounds[index].audioName == name)
                {
                    switch (sounds[index].type)
                    {
                        case AudioType.Music:
                            musicSource.clip = sounds[index].clip;
                            musicSource.Play();
                            break;
                        case AudioType.SFX:
                            sfxSource.PlayOneShot(sounds[index].clip, sounds[index].volume);
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
