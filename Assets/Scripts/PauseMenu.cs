using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    [SerializeField] GameObject pauseButton, pauseMenu;
    [SerializeField] Slider masterSlider, musicSlider, sfxSlider, ambienceSlider;

    bool paused;

    private void Awake() {
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        InputManager.OnPause += PausePressed;
    }

    private void OnDestroy() {
        InputManager.OnPause -= PausePressed;
    }

    private void Start() {
        masterSlider.value = AudioManager.GetMasterVolume();
        musicSlider.value = AudioManager.GetMusicVolume();
        sfxSlider.value = AudioManager.GetSfxVolume();
        ambienceSlider.value = AudioManager.GetAmbienceVolume();
    }

    public void SetMasterVolume(float volume) => AudioManager.SetMasterVolume(volume);
    public void SetMusicVolume(float volume) => AudioManager.SetMusicVolume(volume);
    public void SetSfxVolume(float volume) => AudioManager.SetSfxVolume(volume);
    public void SetAmbienceVolume(float volume) => AudioManager.SetAmbienceVolume(volume);

    public void MainMenu() {
        Unpause();
        // stop all ambience sources
        AudioManager.StopAllAmbienceSources();
        // hide the character and swtich scenes
        BackgroundManager.HideCharacter();
        SceneManager.LoadScene("Main Menu");
    }

    // called by inputmanager when esc pressed
    public void PausePressed() {
        if (paused = !paused) Pause(); // lmao
        else Unpause();
    }

    public void Pause() {
        paused = true;
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void Unpause() {
        paused = false;
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

}
