using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorcycleNPC : MonoBehaviour {

    [SerializeField]
    TextSpawner intro;
    [SerializeField]
    QuestionSpawner question;
    [SerializeField]
    TextSpawner youExplode;
    [SerializeField]
    TextSpawner youSurvive;

    PlayerMovementTest player;

    // when the player enters this collider
    private void OnTriggerEnter2D(Collider2D other) {
        player = other.GetComponent<PlayerMovementTest>();
        player.talking = true;
        intro.onFinished += AskQuestion;
        intro.StartText();
    }

    void AskQuestion() {
        question.OnChoiceSelected += AfterQuestion;
        question.SpawnQuestionBox();
    }

    void AfterQuestion(int choice) {
        TextSpawner dialogueChoice;
        if (choice == 0) {
            dialogueChoice = youExplode;
        }
        else {
            dialogueChoice = youSurvive;
        }
        dialogueChoice.onFinished += Finish;
        dialogueChoice.StartText();
    }

    void Finish() {
        player.talking = false;
    }
}
