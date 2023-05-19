using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorcycleNPC : MonoBehaviour {

    [SerializeField] TextSpawner intro;
    [SerializeField] QuestionSpawner question;
    [SerializeField] TextSpawner youExplode;
    [SerializeField] TextSpawner youSurvive;
    [SerializeField] TextSpawner outsideText;
    [SerializeField] Sprite outside;
    [SerializeField] Sprite outside2;

    private void Start() {
        StartCoroutine(Conversation());
    }

    IEnumerator Conversation() {
        yield return BackgroundManager.FadeIn(outside2);
        yield return BackgroundManager.SlideIn();
        yield return intro.StartText();
        yield return question.StartQuestion();
        TextSpawner response = question.choice == 0 ? youExplode : youSurvive;
        yield return response.StartText();
        yield return BackgroundManager.SlideOut();
        yield return BackgroundManager.FadeOut();
        yield return BackgroundManager.FadeIn(outside);
        yield return BackgroundManager.SlideIn();
        yield return outsideText.StartText();
    }
}
