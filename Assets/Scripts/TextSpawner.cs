using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSpawner : MonoBehaviour {

    [SerializeField]
    GameObject textBoxPrefab;
    [SerializeField]
    TextBoxData[] textBoxDatas;

    int currentTextBoxIdx;
    TextBox currentTextBox;
    
    void Start() {
        NextTextBox();
    }

    void Update() {
        // next text box when space pressed 
        if (Input.GetKeyDown(KeyCode.Space)) {
            NextTextBox();
        }
    }

    public void NextTextBox() {
        // return if current text box is not finished
        if (currentTextBox != null && !currentTextBox.finished) return;
        // display the next text box if there is one
        if (currentTextBoxIdx < textBoxDatas.Length) {
            if (currentTextBox != null) GameObject.Destroy(currentTextBox.gameObject);
            // make a new textbox
            currentTextBox = Instantiate(textBoxPrefab, transform).GetComponent<TextBox>();
            // update the new one with current values
            currentTextBox.SetData(textBoxDatas[currentTextBoxIdx++]);
        }
    }

    [System.Serializable]
    public class TextBoxData {
        public Sprite iconSprite;
        public string message;
        public AudioClip voiceClip;
        public float delayBetweenLetters;
    }
}
