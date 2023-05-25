using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSource : MonoBehaviour {
    
    AudioSource source;

    private void Awake() {
        source = gameObject.AddComponent<AudioSource>();
        source.loop = true;
    }

    public void StartAmbience(AudioClip clip) {
        source.clip = clip;
        source.Play();
        // fade in
        StartCoroutine(Fade(0f, 1f, 2f));
    }

    public void StopAmbience() {
        StartCoroutine(Stop());
    }

    IEnumerator Stop() {
        // fade out
        yield return Fade(1f, 0f, 2f);
        // stop 
        source.Stop();
        GameObject.Destroy(gameObject);
    }

    IEnumerator Fade(float startVolume, float endVolume, float duration = 1) {
        float percent = 0;
        while (percent < 1) {
            source.volume = Mathf.Lerp(startVolume, endVolume, percent);
            percent += Time.deltaTime / duration;
            yield return null;
        }
    }
    
}
