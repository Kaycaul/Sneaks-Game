using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStory : MonoBehaviour {

    [SerializeField] TextSpawner.Conversation[] textBoxDatas;
    [SerializeField] GameObject textBoxPrefab;
    [SerializeField] Sprite outside1;
    [SerializeField] Sprite outside2;
    [SerializeField] Sprite sneaks;
    [SerializeField] Sprite kibbers;

    TextSpawner[] textBoxes;
    
    private void Start() {
        // create textboxes
        textBoxes = new TextSpawner[textBoxDatas.Length];
        for (int i = 0; i < textBoxDatas.Length; i++) {
            textBoxes[i] = TextSpawner.CreateSpawner(textBoxDatas[i].boxes, textBoxPrefab);
            textBoxes[i].gameObject.transform.SetParent(transform);
        }
        StartCoroutine(Story());
    }

    IEnumerator Story() {
        // fade in and show character
        yield return BackgroundManager.FadeIn(outside1);
        yield return BackgroundManager.SlideIn(sneaks);
        // sneaks conversation
        yield return textBoxes[0].StartText();
        // switch to other outside, switch character
        yield return BackgroundManager.SlideOut();
        yield return BackgroundManager.FadeOut();
        yield return BackgroundManager.FadeIn(outside2);
        yield return BackgroundManager.SlideIn(kibbers);
        // kib conversation
        yield return textBoxes[1].StartText();
        // character leaves
        yield return BackgroundManager.SlideOut();
    }
    
}
