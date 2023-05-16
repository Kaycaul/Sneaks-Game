using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorcycleNPC : MonoBehaviour {

    [SerializeField] TextSpawner intro;
    [SerializeField] QuestionSpawner question;
    [SerializeField] TextSpawner youExplode;
    [SerializeField] TextSpawner youSurvive;

    PlayerMovementTest player;

    // redo all this without events theyre stupid
    // when the player enters this collider
    private void OnTriggerEnter2D(Collider2D other) {
        player = other.GetComponent<PlayerMovementTest>();
        player.talking = true;
        StartCoroutine(Conversation());
    }

    IEnumerator Conversation() {
        intro.StartText();
        yield return new WaitUntil(() => intro.finished);
        question.StartQuestion();
        yield return new WaitUntil(() => question.finished);
        TextSpawner response = question.choice == 0 ? youExplode : youSurvive;
        response.StartText();
        yield return new WaitUntil(() => response.finished);
        player.talking = false;
    }
}
