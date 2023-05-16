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

    // read only property for the finished flag
    [HideInInspector] public bool finished {get; private set;} = false;

    private void Start() {
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
        while (messageProgress < message.Length) {
            // add the next letter to the text box
            char character = message[messageProgress++];
            textBoxToUse.text += character;
            // play a sound when a character other than a space is added
            if (!character.Equals(' ') && !character.Equals('\n')) {
                AudioManager.PlaySound(voiceClip);
            }
            // set the finished flag and return if message is finished
            if (messageProgress == message.Length) {
                // show the button prompt
                buttonPrompt.gameObject.SetActive(true);
                finished = true;
                yield break;
            }
            // double delay on newline characters
            if (character.Equals('\n')) {
                yield return new WaitForSeconds(delayBetweenLetters);
            }
            // wait a delay before adding the next letter
            yield return new WaitForSeconds(delayBetweenLetters);
        }
    }

    public void SetData(TextSpawner.TextBoxData newData) {
        showIcon = newData.showIcon;
        if (showIcon) icon.sprite = newData.iconSprite;
        message = newData.message;
        voiceClip = newData.voiceClip;
        delayBetweenLetters = newData.delayBetweenLetters;
    }
}


