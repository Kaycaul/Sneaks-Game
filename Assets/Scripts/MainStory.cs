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
    [SerializeField] Sprite umbrellaCg;
    [SerializeField] Sprite hellCg;
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

    TextSpawner GetSpawner(string title) {
        foreach (TextSpawner spawner in textBoxes) {
            if (spawner.title == title) return spawner;
        }
        Debug.LogError("Couldnt find textbox with title " + title);
        return new TextSpawner();
    }

    IEnumerator ShowConversation(string title) {
        yield return GetSpawner(title).StartText();     
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
        bool saidHello = lastChoice == 0;
        yield return ShowConversation(saidHello ? "Say Hello" : "Say Nothing");
        yield return ShowConversation("Blush");
        yield return BackgroundManager.FadeOut();
        yield return BackgroundManager.SlideOut();
        yield return BackgroundManager.FadeIn(umbrellaCg);
        yield return ShowConversation("Second Umbrella");
        yield return ShowQuestion("Act Normal");
        bool actedNormal = lastChoice == 0;
        yield return ShowConversation(actedNormal ? "Act Normal" : "Act Like A Weirdo");
        // stupid hack because im too lazy to make an entire conversation just for this one variation
        TextSpawner takeUmbrella = GetSpawner("Take Umbrella");
        takeUmbrella.textBoxDatas[4].message += actedNormal ? "(So kind of him...\\ does he really like me?)" : "(It smells even more strongly of him...)";
        yield return takeUmbrella.StartText();
        yield return BackgroundManager.FadeOut();
        yield return BackgroundManager.SlideIn(sneaks);
        yield return BackgroundManager.FadeIn(outside1);
        yield return ShowConversation("Hold Umbrella");
        yield return ShowQuestion("Choose Next Location");
        bool goToSneaksHouse = lastChoice == 0;
        yield return ShowConversation(goToSneaksHouse ? "Go Sneaks House" : "Go Shopping");

    }
    
}
