using UnityEngine;
using System.Collections;

// call the public methods from another script to display the messages
public class TextSpawner : MonoBehaviour {

    [SerializeField] GameObject textBoxPrefab;
    [SerializeField] public TextBoxData[] textBoxDatas;

    int currentTextBoxIdx = 0;
    TextBox currentTextBox;
    bool started = false;
    public bool finished {get; private set;} = false;

    public string title;

    private void Awake() {
        InputManager.OnNextTextbox += NextPressed;
    }

    private void OnDestroy() {
        InputManager.OnNextTextbox -= NextPressed;
    }

    private void NextPressed() {
        if (started) NextTextBox();
    }

    /// <summary>
    /// Restart the text boxes, and display the first one
    /// </summary>
    public IEnumerator StartText() {
        started = true;
        finished = false;
        currentTextBoxIdx = 0;
        NextTextBox();
        yield return new WaitUntil(() => finished);
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

    [System.Serializable]
    public class Conversation {
        public string title;
        public TextBoxData[] boxes;
    }

    public static TextSpawner CreateSpawner(TextBoxData[] data, GameObject textBoxPrefab) {
        GameObject newSpawner = new GameObject("TextSpawner");
        TextSpawner textSpawner = newSpawner.AddComponent<TextSpawner>();
        textSpawner.textBoxPrefab = textBoxPrefab;
        textSpawner.textBoxDatas = data;
        return textSpawner;
    }
}
