using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    [SerializeField] Sprite background;
    [SerializeField] AudioClip menuMusic;

    private void Start() {
        AudioManager.PlayMusic(menuMusic);
        StartCoroutine(BackgroundManager.FadeIn(background));
    }

    public void StartButtonPressed() {
        StartCoroutine(StartGame());
    }

    public void QuitButtonPressed() {
        Application.Quit();
    }

    IEnumerator StartGame() {
        yield return BackgroundManager.FadeOut();
        // switch scene to main story
        SceneManager.LoadScene("Story");
    }    
}
