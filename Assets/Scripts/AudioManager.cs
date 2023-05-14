using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {


    public static AudioManager instance;

    static AudioSource sfxSource;
    static AudioSource[] musicSources = new AudioSource[2];
    static int activeMusicSourceIndex = 0;

    public static void PlaySound(AudioClip clip) {
        if (clip == null) {
            Debug.LogError("Tried to play null sound clip!");
            return;
        }
        sfxSource.PlayOneShot(clip);
    }

    public static void PlayMusic(AudioClip clip, float fadeDuration = 1) {
        if (clip == null) {
            Debug.LogError("Tried to play null music clip!");
            return;
        }
        
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();

        instance.StartCoroutine(instance.CrossFade(fadeDuration));
    }

    IEnumerator CrossFade(float duration) {
        float percent = 0;
        while (percent < 1) {
            percent += Time.deltaTime / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, 1, percent);
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(1, 0, percent);
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
