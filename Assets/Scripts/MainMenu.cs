using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    [SerializeField] AudioClip menuMusic;
    [SerializeField] RawImage backgroundImage;
    [SerializeField] Color fadeColor1, fadeColor2;
    [SerializeField] Sprite guy;

    bool started = false;

    private void Start() {
        StartCoroutine(BackgroundManager.FadeIn());
        AudioManager.PlayMusic(menuMusic);
    }

    private void Update() {
        // move the background
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // subtract this or it will go backward
        backgroundImage.uvRect = new Rect(backgroundImage.uvRect.position - mousePos * Time.deltaTime * 0.1f, backgroundImage.uvRect.size);
        // alternate between two colors
        Color currentColor = Color.Lerp(fadeColor1, fadeColor2, Mathf.PingPong(Time.time / 3, 1));
        backgroundImage.color = currentColor;
    }

    public void StartButtonPressed() {
        StartCoroutine(StartGame());
    }

    public void QuitButtonPressed() {
        StartCoroutine(Quit());
    }

    IEnumerator Quit() {
        backgroundImage.gameObject.SetActive(false);
        BackgroundManager.UpdateBackground(guy);
        yield return BackgroundManager.FadeOut();
        Application.Quit();
    }

    IEnumerator StartGame() {
        if (started) yield break;
        started = true;
        yield return new WaitUntil(() => !BackgroundManager.IsFading());
        yield return BackgroundManager.FadeOut();
        // switch scene to main story
        SceneManager.LoadScene("Story");
    }    
}
