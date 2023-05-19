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

    private void Start() {
        StartCoroutine(Conversation());
    }

    IEnumerator Conversation() {
        BackgroundManager.UpdateBackground(outside2);
        BackgroundManager.FadeIn();
        yield return new WaitUntil(() => !BackgroundManager.isFading());
        intro.StartText();
        yield return new WaitUntil(() => intro.finished);
        question.StartQuestion();
        yield return new WaitUntil(() => question.finished);
        TextSpawner response = question.choice == 0 ? youExplode : youSurvive;
        response.StartText();
        yield return new WaitUntil(() => response.finished);
        BackgroundManager.FadeOut();
        yield return new WaitUntil(() => !BackgroundManager.isFading());
        BackgroundManager.UpdateBackground(outside);
        BackgroundManager.FadeIn();
    }
}
