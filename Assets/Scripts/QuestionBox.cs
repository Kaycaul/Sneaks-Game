using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionBox : MonoBehaviour {

    public event System.Action<int> OnChoiceSelected;

    [SerializeField]
    TMP_Text textBox;
    [SerializeField]
    AudioSource voiceBeeper;

    public string[] choices;
    public string message;
    public AudioClip voiceClip;
    public float delayBetweenLetters;

    // read only property for the finished flag
    [HideInInspector]
    public bool finished {get; private set;} = false;

    bool readyToChoose = false;
    int currentChoice = 0;

    private void Start() {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText() {
        textBox.text = "";
        int messageProgress = 0;
        // display the message
        while (!readyToChoose) {
            // add the next letter to the text box
            char character = message[messageProgress++];
            textBox.text += character;
            // play a sound when a character other than a space is added
            if (!character.Equals(' ')) {
                voiceBeeper.PlayOneShot(voiceClip);
            }
            // set the finished flag and return if message is finished
            if (messageProgress == message.Length) {
                readyToChoose = true;
            }
            // double delay on newline characters
            if (character.Equals('\n')) {
                yield return new WaitForSeconds(delayBetweenLetters);
            }
            // wait a delay before adding the next letter
            yield return new WaitForSeconds(delayBetweenLetters);
        }
        // display the questions and let the player choose
        while (!finished) {

            // stop when the player makes a choice
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
                if (OnChoiceSelected != null) OnChoiceSelected(currentChoice);
                finished = true;
            }

            // increase and decrease the current choice on arrow keys
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                currentChoice = Mathf.Max(0, currentChoice - 1);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                currentChoice = Mathf.Min(choices.Length - 1, currentChoice + 1);
            }

            if (readyToChoose) {
                // display the choices
                textBox.text = message;
                for (int i = 0; i < choices.Length; i++) {
                    textBox.text += (currentChoice == i ? "\r\n>" : "\r\n") + choices[i];
                }
            }
            yield return null;
        }
        GameObject.Destroy(gameObject);
    }

    public class QuestionBoxData {
        public string message;
        public string[] choices;
        public AudioClip voiceClip;
        public float delayBetweenLetters;
    }



    public void SetData(QuestionBoxData newData) {
        message = newData.message;
        choices = newData.choices;
        voiceClip = newData.voiceClip;
        delayBetweenLetters = newData.delayBetweenLetters;
    }
}


