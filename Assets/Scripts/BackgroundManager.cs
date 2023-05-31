using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour {

    [SerializeField] GameObject backgroundObjectPrefab; // background image
    [SerializeField] GameObject characterSpritePrefab; // image of the character you are talking to
    [SerializeField] GameObject blackScreenPrefab; // black screen usually transparent, for fade transitions
    [SerializeField] AnimationCurve fadeCurve; // for the black screen fading
    [SerializeField] AnimationCurve slideCurve; // for character sprites sliding in from the side

    public static BackgroundManager instance;

    Image background;
    Image characterSprite;
    Image blackScreen;
    bool fading = false;
    bool sliding = false;
    public static bool IsFading() => instance.fading;
    public static bool IsSliding() => instance.sliding;

    private const int SLIDE_DISTANCE = 400;

    public static void UpdateBackground(Sprite sprite) {
        instance.background.sprite = sprite;
    }

    public static IEnumerator FadeIn() {
        yield return instance.FadeBetween(Color.black, Color.clear);
    }

    public static IEnumerator FadeIn(Sprite sprite) {
        UpdateBackground(sprite);
        yield return FadeIn();
    }

    public static IEnumerator FadeOut() {
        Debug.Log("Fading out");
        yield return instance.FadeBetween(Color.clear, Color.black);
    }

    public static void UpdateCharacter(Sprite sprite) {
        instance.characterSprite.sprite = sprite;
    }

    private static float CharacterWidth() => instance.characterSprite.sprite.rect.width;

    public static void HideCharacter() {
        instance.characterSprite.rectTransform.anchoredPosition = new Vector2(-CharacterWidth(), instance.characterSprite.rectTransform.anchoredPosition.y);
    }

    public static IEnumerator SlideIn() {
        yield return instance.SlideBetween(-CharacterWidth(), SLIDE_DISTANCE);
    }

    public static IEnumerator SlideIn(Sprite sprite) {
        UpdateCharacter(sprite);
        yield return SlideIn();
    }

    public static IEnumerator SlideOut() {
        yield return instance.SlideBetween(SLIDE_DISTANCE, -CharacterWidth());
    }

    IEnumerator FadeBetween(Color start, Color end) {
        if (IsFading()) yield break;
        fading = true;
        float t = 0f;
        while (t < 1f) {
            blackScreen.color = Color.Lerp(start, end, fadeCurve.Evaluate(t));
            t += Time.deltaTime;
            yield return null;
        }
        fading = false;
    }

    IEnumerator SlideBetween(float start, float end) {
        if (IsSliding()) yield break;
        sliding = true;
        float t = 0f;
        while (t < 1f) {
            float distance = Mathf.LerpUnclamped(start, end, slideCurve.Evaluate(t));
            characterSprite.rectTransform.anchoredPosition = new Vector2(distance, characterSprite.rectTransform.anchoredPosition.y);
            characterSprite.color = Color.Lerp(Color.clear, Color.white, (start < end ? t : 1 - t));
            t += Time.deltaTime * 1.5f;
            yield return null;
        }
        sliding = false;
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
        GameObject newCharacterSprite = Instantiate(characterSpritePrefab);
        newCharacterSprite.transform.SetParent(transform);
        characterSprite = newCharacterSprite.transform.GetChild(0).GetComponent<Image>();
        GameObject newBlackScreen = Instantiate(blackScreenPrefab);
        newBlackScreen.transform.SetParent(transform);
        blackScreen = newBlackScreen.transform.GetChild(0).GetComponent<Image>();

        HideCharacter();
    }

}
    

