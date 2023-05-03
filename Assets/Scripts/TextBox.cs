using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextBox : MonoBehaviour {

    [SerializeField]
    Image icon;
    [SerializeField]
    TMP_Text textBox;
    [SerializeField]
    string message;
    [SerializeField]
    float delayBetweenLetters;

    private void Start() {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText() {
        textBox.text = "";
        int messageProgress = 0;
        while (messageProgress < message.Length) {
            // add the next letter to the text box
            textBox.text += message[messageProgress++];
            // play a sound when a character other than a space is added
            
            // wait a delay before adding the next letter
            yield return new WaitForSeconds(delayBetweenLetters);
        }
    }
}
