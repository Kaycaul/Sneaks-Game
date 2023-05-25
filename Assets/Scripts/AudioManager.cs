using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inspired by sebastian lagues audiomanager script 
public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    static AudioSource sfxSource;
    static AudioSource[] musicSources = new AudioSource[2];
    static int activeMusicSourceIndex = 0;

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

        instance.StartCoroutine(CrossFade(fadeDuration, musicSources[activeMusicSourceIndex], musicSources[1 - activeMusicSourceIndex]));
    }

    // play an ambient sound
    // note: ambient sounds should be edited externally to fade in and out, or otherwise loop seamlessly
    public static AmbienceSource PlayAmbience(AudioClip clip) {
        if (clip == null) {
            Debug.LogError("Tried to play null ambient clip!");
            return null;
        }
        AmbienceSource ambienceSource = new GameObject("Ambience Source").AddComponent<AmbienceSource>();
        ambienceSource.StartAmbience(clip);
        return ambienceSource; // caller should call StopAmbience() when done using
    }

    // fade between two audio sources
    static IEnumerator CrossFade(float duration, AudioSource source1, AudioSource source2) {
        float percent = 0;
        while (percent < 1) {
            percent += Time.deltaTime / duration;
            source1.volume = Mathf.Lerp(0, 1, percent);
            source2.volume = Mathf.Lerp(1, 0, percent);
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
    }
    
}
