using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionSpawner : MonoBehaviour {

    [SerializeField]
    GameObject questionBoxPrefab;
    [SerializeField]
    QuestionBox.QuestionBoxData questionBoxData;

    public event System.Action<int> OnChoiceSelected;

    public void SpawnQuestionBox() {
        QuestionBox questionBox = Instantiate(questionBoxPrefab).GetComponent<QuestionBox>();
        questionBox.SetData(questionBoxData);
        questionBox.OnChoiceSelected += UpdateChoice;
    }

    void UpdateChoice(int choice) {
        if (OnChoiceSelected != null) OnChoiceSelected(choice);
    }
}
