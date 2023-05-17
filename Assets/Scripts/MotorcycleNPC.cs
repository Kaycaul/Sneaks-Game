using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorcycleNPC : MonoBehaviour {

    [SerializeField] TextSpawner intro;
    [SerializeField] QuestionSpawner question;
    [SerializeField] TextSpawner youExplode;
    [SerializeField] TextSpawner youSurvive;
    [SerializeField] Sprite outside;
    [SerializeField] Sprite outside2;

    /*
    PlayerMovementTest player;
    // when the player enters this collider
    private void OnTriggerEnter2D(Collider2D other) {
        player = other.GetComponent<PlayerMovementTest>();
        player.talking = true;
        StartCoroutine(Conversation());
    }
    */
    
    private void Start() {
        StartCoroutine(Conversation());
    }

    IEnumerator Conversation() {
        BackgroundManager.UpdateBackground(outside2);
        intro.StartText();
        yield return new WaitUntil(() => intro.finished);
        question.StartQuestion();
        yield return new WaitUntil(() => question.finished);
        TextSpawner response = question.choice == 0 ? youExplode : youSurvive;
        response.StartText();
        yield return new WaitUntil(() => response.finished);
        BackgroundManager.UpdateBackground(outside);
        // player.talking = false;
    }
}
