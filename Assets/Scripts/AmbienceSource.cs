using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSource : MonoBehaviour {
    
    AudioSource source;
    float currentVolume;
    
    public void SetVolume(float newVolume) {
        currentVolume = newVolume;
        source.volume = newVolume;
    }

    private void Awake() {
        source = gameObject.AddComponent<AudioSource>();
        source.loop = true;
    }

    public void StartAmbience(AudioClip clip) {
        source.clip = clip;
        source.Play();
        StartCoroutine(Fade(0f, currentVolume, 1f)); // fade in over the course of a second
        source.volume = currentVolume;
    }

    public void StopAmbience() {
        StartCoroutine(Stop());
    }

    IEnumerator Stop() {
        // fade out
        yield return Fade(currentVolume, 0f, 2f);
        // stop 
        source.Stop();
        GameObject.Destroy(gameObject);
    }

    private void OnDestroy() {
        AudioManager.OnAmbienceVolumeChanged -= SetVolume;
    }
    
    // varies the volume over a specified duration
    IEnumerator Fade(float startVolume, float endVolume, float duration = 1) {
        float percent = 0;
        while (percent < 1) {
            source.volume = Mathf.Lerp(startVolume, endVolume, percent);
            percent += Time.deltaTime / duration;
            yield return null;
        }
    }
    
}
