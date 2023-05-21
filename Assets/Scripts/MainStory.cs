using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStory : MonoBehaviour {

    [SerializeField] TextSpawner.Conversation[] textBoxDatas;
    [SerializeField] QuestionBox.QuestionBoxData[] questionBoxDatas;
    [SerializeField] GameObject textBoxPrefab;
    [SerializeField] GameObject questionBoxPrefab;
    [SerializeField] Sprite outside1;
    [SerializeField] Sprite outside2;
    [SerializeField] Sprite sneaks;
    [SerializeField] Sprite kibbers;

    TextSpawner[] textBoxes;
    QuestionSpawner[] questionBoxes;

    int lastChoice = -1;
    
    private void Start() {
        // create textboxes
        textBoxes = new TextSpawner[textBoxDatas.Length];
        for (int i = 0; i < textBoxDatas.Length; i++) {
            textBoxes[i] = TextSpawner.CreateSpawner(textBoxDatas[i].boxes, textBoxPrefab);
            textBoxes[i].title = textBoxDatas[i].title;
            textBoxes[i].gameObject.transform.SetParent(transform);
        }
        // create questions
        questionBoxes = new QuestionSpawner[questionBoxDatas.Length];
        for (int i = 0; i < questionBoxDatas.Length; i++) {
            questionBoxes[i] = QuestionSpawner.CreateQuestionSpawner(questionBoxPrefab, questionBoxDatas[i]);
            questionBoxes[i].gameObject.transform.SetParent(transform);
        }
        StartCoroutine(Story());
    }

    IEnumerator ShowConversation(string title) {
        // find the textspawner with that title
        foreach (TextSpawner spawner in textBoxes) {
            if (spawner.title == title) {
                yield return spawner.StartText();
                yield break;
            }
        }
        // couldnt find it
        Debug.LogError("Couldnt find textbox with title " + title);
    }

    IEnumerator ShowQuestion(string title) {
        // find the questionbox with that title
        foreach (QuestionSpawner spawner in questionBoxes) {
            if (spawner.title == title) {
                yield return spawner.StartQuestion();
                lastChoice = spawner.choice;
                yield break;
            }
        }
        // couldnt find it
        Debug.LogError("Couldnt find questionbox with title " + title);
    }

    IEnumerator Story() {
        yield return BackgroundManager.FadeIn(outside1);
        yield return ShowConversation("Intro");
        yield return BackgroundManager.SlideIn(sneaks);
        yield return ShowConversation("Sneaks Appears");
        yield return ShowQuestion("Say Hello");
        yield return ShowConversation(lastChoice == 0 ? "Say Hello" : "Say Nothing");
        yield return ShowConversation("Blush");

    }
    
}
