using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionSpawner : MonoBehaviour {

    [SerializeField] GameObject questionBoxPrefab;
    [SerializeField] QuestionBox.QuestionBoxData questionBoxData;

    public int choice {get; private set;} = -1;
    public bool finished {get; private set;} = false;

    public string title {get; private set;}

    public IEnumerator StartQuestion() {
        choice = -1;
        finished = false;
        QuestionBox questionBox = Instantiate(questionBoxPrefab).GetComponent<QuestionBox>();
        questionBox.SetData(questionBoxData);
        questionBox.OnChoiceSelected += (newChoice) => {choice = newChoice; finished = true;};
        yield return new WaitUntil(() => finished);
    }

    public static QuestionSpawner CreateQuestionSpawner(GameObject questionboxPrefab, QuestionBox.QuestionBoxData questionBoxData) {
        QuestionSpawner newSpawner = new GameObject("QuestionSpawner").AddComponent<QuestionSpawner>();
        newSpawner.questionBoxPrefab = questionboxPrefab;
        newSpawner.questionBoxData = questionBoxData;
        newSpawner.title = questionBoxData.title;
        return newSpawner;
    }
}
