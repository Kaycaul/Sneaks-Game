using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextBox : MonoBehaviour {

    [SerializeField] Image icon;
    [SerializeField] TMP_Text textBox;
    [SerializeField] TMP_Text bigTextBox;
    [SerializeField] Image buttonPrompt;

    Sprite iconSprite;
    AudioClip voiceClip;
    TMP_Text textBoxToUse;
    string message;
    float delayBetweenLetters;
    bool showIcon;
    float oldDelay;
    bool skipped = false;

    // read only property for the finished flag
    [HideInInspector] public bool finished {get; private set;} = false;

    private void Start() {
        // cheat code
        if (Input.GetKey(KeyCode.Delete)) {
            delayBetweenLetters = 0.00001f;
        }
        // hide certain elements not needed
        buttonPrompt.gameObject.SetActive(false);
        icon.gameObject.SetActive(showIcon);
        textBox.gameObject.SetActive(showIcon);
        bigTextBox.gameObject.SetActive(!showIcon);
        textBoxToUse = showIcon ? textBox : bigTextBox;
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText() {
        textBoxToUse.text = "";
        int messageProgress = 0;
        // each letter is added to the text box, one at a time.
        // there are 3 exceptions, none of which play sound:
        // spaces
        // newlines, which process for 3 times longer
        // backslashes, used as a delay, dont render
        while (messageProgress < message.Length) {
            // add the next letter to the text box
            char character = message[messageProgress++];
            // add the character, unless it is a delay character
            if (!character.Equals('\\')) textBoxToUse.text += character;
            // play a sound when a character other than a space is added
            if (!character.Equals(' ') && !character.Equals('\n') && !character.Equals('\\')) {
                if (!skipped) AudioManager.PlaySound(voiceClip);
            }
            // set the finished flag and return if message is finished
            if (messageProgress == message.Length) {
                // show the button prompt
                buttonPrompt.gameObject.SetActive(true);
                finished = true;
                yield break;
            }
            // triple delay on special characters
            if (character.Equals('\n') || character.Equals('\\')) {
                yield return new WaitForSeconds(2*delayBetweenLetters);
            }
            // wait a delay before adding the next letter
            yield return new WaitForSeconds(delayBetweenLetters);
        }
    }

    public void Skip() {
        delayBetweenLetters = 0;
        skipped = true;
    }

    public void SetData(TextSpawner.TextBoxData newData) {
        showIcon = newData.showIcon;
        if (showIcon) icon.sprite = newData.iconSprite;
        message = newData.message;
        voiceClip = newData.voiceClip;
        delayBetweenLetters = newData.delayBetweenLetters;
    }
}


