using UnityEngine;

// call the public methods from another script to display the messages
public class TextSpawner : MonoBehaviour {

    [SerializeField] GameObject textBoxPrefab;
    [SerializeField] TextBoxData[] textBoxDatas;

    int currentTextBoxIdx = 0;
    TextBox currentTextBox;
    bool started = false;
    public bool finished {get; private set;} = false;

    private void Update() {
        // next on space pressed
        if (Input.GetKeyDown(KeyCode.Space) && started) NextTextBox();
    }

    /// <summary>
    /// Restart the text boxes, and display the first one
    /// </summary>
    public void StartText() {
        started = true;
        finished = false;
        currentTextBoxIdx = 0;
        NextTextBox();
    }

    /// <summary>
    /// Display the next text box, or stop if there isnt one left.
    /// Should be called when the player clicks as they read.
    /// </summary>
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
            if (currentTextBox != null) GameObject.Destroy(currentTextBox.gameObject);
            currentTextBox = null;
            finished = true;
        }
    }

    [System.Serializable]
    public class TextBoxData {
        public Sprite iconSprite;
        public bool showIcon = true;
        [TextArea] public string message;
        public AudioClip voiceClip;
        public float delayBetweenLetters;
    }
}
