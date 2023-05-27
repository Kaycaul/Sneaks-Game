using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inspired by sebastian lagues audiomanager script 
public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    static AudioSource sfxSource;
    static AudioSource[] musicSources = new AudioSource[2];
    static int activeMusicSourceIndex = 0;
    static float masterVolume, musicVolume, sfxVolume, ambienceVolume;
    public static float GetMasterVolume() => masterVolume;
    public static float GetMusicVolume() => musicVolume;
    public static float GetSfxVolume() => sfxVolume;
    public static float GetAmbienceVolume() => ambienceVolume;

    static event System.Action<float> OnAmbienceVolumeChanged;

    // update the volume of sources, should be called by ui
    public static void SetMasterVolume(float volume) {
        masterVolume = volume;
        SetMusicVolume(musicVolume);
        SetSfxVolume(sfxVolume);
        SetAmbienceVolume(ambienceVolume);
    }
    
    public static void SetMusicVolume(float volume) {
        musicVolume = volume;
        foreach (AudioSource source in musicSources) {
            source.volume = musicVolume * masterVolume;
        }
    }

    public static void SetSfxVolume(float volume) {
        sfxVolume = volume;
        sfxSource.volume = sfxVolume * masterVolume;
    }

    public static void SetAmbienceVolume(float volume) {
        ambienceVolume = volume;
        // update the volume of every subscribed source
        if (OnAmbienceVolumeChanged != null) OnAmbienceVolumeChanged(ambienceVolume * masterVolume);
    }

    // plays a sound just once
    public static void PlaySound(AudioClip clip) {
        if (clip == null) {
            Debug.LogError("Tried to play null sound clip!");
            return;
        }
        sfxSource.PlayOneShot(clip);
    }

    // starts a new song and stops the previous one, by fading it out
    public static void PlayMusic(AudioClip clip, float fadeDuration = 1) {
        if (clip == null) {
            Debug.LogError("Tried to play null music clip!");
            return;
        }
        
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();

        instance.StartCoroutine(CrossFade(
            fadeDuration, 
            musicSources[activeMusicSourceIndex], 
            musicSources[1 - activeMusicSourceIndex],
            musicVolume * masterVolume
            ));
    }

    // play an ambient sound
    // note: ambient sounds should be edited externally to fade in and out, or otherwise loop seamlessly
    public static AmbienceSource PlayAmbience(AudioClip clip) {
        if (clip == null) {
            Debug.LogError("Tried to play null ambient clip!");
            return null;
        }
        AmbienceSource ambienceSource = new GameObject("Ambience Source").AddComponent<AmbienceSource>();
        // set the volume and subscribe it for future updates
        ambienceSource.SetVolume(ambienceVolume * masterVolume);
        OnAmbienceVolumeChanged += ambienceSource.SetVolume;
        ambienceSource.StartAmbience(clip);
        return ambienceSource; // caller should call StopAmbience() when done using
    }

    // fade between two audio sources
    static IEnumerator CrossFade(float duration, AudioSource source1, AudioSource source2, float maxVolume) {
        float percent = 0;
        while (percent < 1) {
            percent += Time.deltaTime / duration;
            source1.volume = Mathf.Lerp(0, maxVolume, percent);
            source2.volume = Mathf.Lerp(maxVolume, 0, percent);
            yield return null;
        }
    }

    private void Awake() {

        // delete or persist, there can be only one
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        // set up music sources, and parent them
        for (int i = 0; i < musicSources.Length; i++) {
            GameObject newSource = new GameObject("Music Source " + i);
            musicSources[i] = newSource.AddComponent<AudioSource>();
            musicSources[i].loop = true;
            newSource.transform.parent = transform;
        }
        // same with sound source
        GameObject newSfxSource = new GameObject("Sfx Source");
        newSfxSource.transform.parent = transform;
        sfxSource = newSfxSource.AddComponent<AudioSource>();

        // temp for volume testing, might keep it tho
        SetMasterVolume(0.8f);
        SetSfxVolume(0.8f);
        SetMusicVolume(0.2f);
        SetAmbienceVolume(0.4f);
        // \temp
    }
    
}
