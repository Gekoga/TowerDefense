using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public enum AudioChannel {Master, SFX, Music};

    public float masterVolume {get; private set;}
    public float sfxVolume {get; private set;}
    public float musicVolume {get; private set;}

    [Header("Audiosources")]
    AudioSource[] musicSources;
    AudioSource sfx2DSource;

    [Header("Ints")]
    int activeMusicSourceIndex;
    public static AudioManager instance;

    [Header("Ints")]
    Transform audioListener;
    Transform playerT;

    AudioLibrary library;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            library = GetComponent<AudioLibrary>();

            musicSources = new AudioSource[2];
            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new GameObject("Music source " + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;
            }
            GameObject newSFX2DSource = new GameObject("2D SFX source");
            sfx2DSource = newSFX2DSource.AddComponent<AudioSource>();
            newSFX2DSource.transform.parent = transform;


            audioListener = FindObjectOfType<AudioListener>().transform;
            if (FindObjectOfType<CameraScript>())
            {
                playerT = FindObjectOfType<CameraScript>().transform;
            }
            masterVolume = PlayerPrefs.GetFloat("Master volume", 0);
            sfxVolume = PlayerPrefs.GetFloat("SFX volume", 0);
            musicVolume = PlayerPrefs.GetFloat("Music volume", 0);
            PlayerPrefs.Save();
        }
    }

    void Update()
    {
        if (playerT != null)
        {
            audioListener.position = playerT.position;
        }
    }

    public void SetVolume(float Volume, AudioChannel channel)
    {
        switch (channel)
        {
            case AudioChannel.Master:
                masterVolume = Volume;
                break;

            case AudioChannel.SFX:
                sfxVolume = Volume;
                break;

            case AudioChannel.Music:
                musicVolume = Volume;
                break;
        }

        musicSources[0].volume = musicVolume * masterVolume;
        musicSources[1].volume = musicVolume * masterVolume;

        PlayerPrefs.SetFloat("Master volume", masterVolume);
        PlayerPrefs.SetFloat("SFX volume", sfxVolume);
        PlayerPrefs.SetFloat("Music volume", musicVolume);
        PlayerPrefs.Save();
    }

    public void PlaySound(AudioClip clip, Vector3 pos)
    {

        //maybe remmove if (clip != null) and put audiolistener on camera and remove child of AudioManager if not required
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, sfxVolume * masterVolume);
        }
    }

    public void PlaySound(string soundName, Vector3 pos)
    {
        PlaySound(library.GetClipFromName(soundName), pos);
    }
    
    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(library.GetClipFromName(soundName), sfxVolume * masterVolume);
    }

    
    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();
        StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    }

    IEnumerator AnimateMusicCrossfade(float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolume * masterVolume, percent);
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolume * masterVolume, 0, percent);
            yield return null;
        }
    }
}
