using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextBox : MonoBehaviour {

    [SerializeField]
    Image icon;
    [SerializeField]
    TMP_Text textBox;
    [SerializeField]
    AudioSource voiceBeeper;
    [SerializeField]
    Image buttonPrompt;

    public Sprite iconSprite;
    public string message;
    public AudioClip voiceClip;
    public float delayBetweenLetters;

    // read only property for the finished flag
    [HideInInspector]
    public bool finished {get; private set;} = false;

    private void Start() {
        buttonPrompt.gameObject.SetActive(false);
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText() {
        textBox.text = "";
        int messageProgress = 0;
        while (messageProgress < message.Length) {
            // add the next letter to the text box
            char character = message[messageProgress++];
            textBox.text += character;
            // play a sound when a character other than a space is added
            if (!character.Equals(' ') && !character.Equals('\n')) {
                voiceBeeper.PlayOneShot(voiceClip);
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
        icon.sprite = newData.iconSprite;
        message = newData.message;
        voiceClip = newData.voiceClip;
        delayBetweenLetters = newData.delayBetweenLetters;
    }
}


