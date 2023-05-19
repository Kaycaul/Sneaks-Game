using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour {

    [SerializeField] GameObject backgroundObjectPrefab;

    public static BackgroundManager instance;

    Image background;
    bool fading = false;

    // create a new background image with the provided sprite
    public static void UpdateBackground(Sprite sprite) {
        instance.background.sprite = sprite;
    }

    public static void FadeIn() {
        if (isFading()) return;
        instance.fading = true;
        instance.StartCoroutine(instance.FadeBetween(Color.black, Color.white));
    }

    public static void FadeOut() {
        if (isFading()) return;
        instance.fading = true;
        instance.StartCoroutine(instance.FadeBetween(Color.white, Color.black));
    }

    public static bool isFading() {
        return instance.fading;
    }

    IEnumerator FadeBetween(Color start, Color end) {
        float t = 0f;
        while (t < 1f) {
            instance.background.color = Color.Lerp(start, end, t);
            t += Time.deltaTime;
            yield return null;
        }
        fading = false;
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        GameObject backgroundObject = Instantiate(backgroundObjectPrefab);    
        backgroundObject.transform.SetParent(transform);
        background = backgroundObject.transform.GetChild(0).GetComponent<Image>();
    }

}
    

