using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TextBox : MonoBehaviour {

    [SerializeField]
    Image icon;
    [SerializeField]
    TMP_Text textBox;
    [SerializeField]
    AudioSource voiceBeeper;

    public Sprite iconSprite;
    public string message;
    public AudioClip voiceClip;
    public float delayBetweenLetters;

    // read only property for the finished flag
    [HideInInspector]
    public bool finished {get; private set;} = false;

    private void Start() {
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
            if (!character.Equals(' ')) {
                voiceBeeper.PlayOneShot(voiceClip);
            }
            // set the finished flag and return if message is finished
            if (messageProgress == message.Length) {
                finished = true;
                yield break;
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


