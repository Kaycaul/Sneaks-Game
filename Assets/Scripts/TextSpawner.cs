using UnityEngine;

// call the public methods from another script to display the messages
public class TextSpawner : MonoBehaviour {

    [SerializeField]
    GameObject textBoxPrefab;
    [SerializeField]
    TextBoxData[] textBoxDatas;

    int currentTextBoxIdx = 0;
    TextBox currentTextBox;

    public bool finished { get; private set; } = false;

    public void StartText() {
        finished = false;
        currentTextBoxIdx = 0;
        NextTextBox();
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
        } else {
            finished = true;
        }
    }

    public void Stop() {
        GameObject.Destroy(currentTextBox.gameObject);
        currentTextBox = null;
        currentTextBoxIdx = 0;
    }

    [System.Serializable]
    public class TextBoxData {
        public Sprite iconSprite;
        [TextArea]
        public string message;
        public AudioClip voiceClip;
        public float delayBetweenLetters;
    }
}
