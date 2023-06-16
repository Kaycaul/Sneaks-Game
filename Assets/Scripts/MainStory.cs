using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainStory : MonoBehaviour {

    [SerializeField] TextSpawner.Conversation[] textBoxDatas;
    [SerializeField] QuestionBox.QuestionBoxData[] questionBoxDatas;
    [SerializeField] GameObject textBoxPrefab;
    [SerializeField] GameObject questionBoxPrefab;
    [SerializeField] Sprite outside1, outside2, umbrellaCg, 
    weirdUmbrellaCg, hellCg, sneaks, kibbers, kibHappyCg, kibWtfCg, kibShopBackground, 
    sneaksHouse, sneaksHouseEvil, sneaksRoom, sneaksRoomCg, peek1, peek2, black;
    [SerializeField] AudioClip outsideMusic, shoppingMusic, houseMusic, hellMusic;
    [SerializeField] AudioClip rain, crowd, cars, insideRain;
    [SerializeField] AudioClip metalPipe;

    TextSpawner[] textBoxes;
    QuestionSpawner[] questionBoxes;

    bool purchasedAnvil = false;
    bool purchasedChalk = false;
    bool purchasedPotion = false;
    bool skippedShopping;

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
        //StartCoroutine(Shopping());
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
        BackgroundManager.UpdateBackground(black);
        yield return BackgroundManager.FadeOut();
        AudioManager.PlayMusic(outsideMusic);
        AmbienceSource crowdSource = AudioManager.PlayAmbience(crowd);
        AmbienceSource insideRainSource = AudioManager.PlayAmbience(insideRain);
        yield return ShowConversation("Intro");
        yield return BackgroundManager.FadeIn(outside1);
        crowdSource.StopAmbience();
        insideRainSource.StopAmbience();
        AmbienceSource outsideRainSource = AudioManager.PlayAmbience(rain);
        AmbienceSource trafficSource = AudioManager.PlayAmbience(cars);
        yield return ShowConversation("Outside");
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
        if (!actedNormal) BackgroundManager.UpdateBackground(weirdUmbrellaCg);
        yield return ShowConversation(actedNormal ? "Act Normal" : "Act Like A Weirdo");
        BackgroundManager.UpdateBackground(umbrellaCg);
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
        skippedShopping = goToSneaksHouse;
        AudioManager.StopAllAmbienceSources();
        yield return BackgroundManager.FadeOut();
        yield return goToSneaksHouse ? SneaksHouse() : Shopping();
    }

    IEnumerator Shopping() {
        BackgroundManager.HideCharacter();
        AudioManager.StopAllAmbienceSources();
        AudioManager.PlayAmbience(insideRain);
        AudioManager.PlayMusic(shoppingMusic);
        yield return BackgroundManager.FadeIn(kibShopBackground);
        yield return ShowConversation("Kib Hi");
        yield return BackgroundManager.SlideIn(kibbers);
        yield return ShowConversation("Shop Bell");
        yield return BackgroundManager.FadeOut();
        BackgroundManager.HideCharacter();
        yield return BackgroundManager.FadeIn(kibHappyCg);
        bool shopping = true;
        while (shopping) {
            BackgroundManager.UpdateBackground(kibHappyCg);
            yield return ShowConversation("Kib Store Start");
            yield return ShowQuestion("Kib Store");
            if (lastChoice == 0) {
                if (purchasedAnvil) BackgroundManager.UpdateBackground(kibWtfCg);
                yield return ShowConversation(purchasedAnvil ? "Already Purchased" : "Buy Anvil");
                purchasedAnvil = true;
            }
            else if (lastChoice == 1) {
                if (purchasedChalk) BackgroundManager.UpdateBackground(kibWtfCg);
                yield return ShowConversation(purchasedChalk ? "Already Purchased" : "Buy Chalk");
                purchasedChalk = true;
            }
            else if (lastChoice == 2) {
                if (purchasedPotion) BackgroundManager.UpdateBackground(kibWtfCg);
                yield return ShowConversation(purchasedPotion ? "Already Purchased" : "Buy Potion");
                purchasedPotion = true;
            }
            else shopping = false;
        }
        yield return BackgroundManager.FadeOut();
        yield return BackgroundManager.SlideIn(kibbers);
        yield return BackgroundManager.FadeIn(kibShopBackground);
        yield return ShowConversation("Done Shopping");
        yield return ShowQuestion("Follow Him");
        bool followSneaks = lastChoice == 0;
        yield return ShowConversation(followSneaks ? "Follow Sneaks" : "Talk To Kib");
        yield return BackgroundManager.FadeOut();
        yield return ShowConversation("Call Sneaks");
        //yield return BackgroundManager.FadeOut(); // outside sneaks house fades out
        BackgroundManager.HideCharacter();
        AudioManager.StopAllAmbienceSources();
        yield return SneaksHouse();
    }

    IEnumerator SneaksHouse() {
        AudioManager.PlayAmbience(rain);
        AudioManager.PlayMusic(houseMusic);
        yield return BackgroundManager.SlideIn(sneaks);
        yield return ShowConversation("Arrive At House");
        yield return BackgroundManager.FadeIn(skippedShopping ? sneaksHouseEvil : sneaksHouse);
        AudioManager.StopAllAmbienceSources();
        AudioManager.PlayAmbience(insideRain);
        if (skippedShopping) {
            GetSpawner("Inside House").textBoxDatas[0].message = "Inside the house, it smells like brimstone and burning plastic.";
        }
        yield return ShowConversation("Inside House");
        yield return BackgroundManager.SlideOut();
        yield return ShowConversation("Waiting In House");
        bool wentDownTrapdoor = false;
        if (skippedShopping) {
            yield return ShowConversation("Trapdoor");
            yield return ShowQuestion("Closer Look");
            if (lastChoice == 1) goto Break;
            yield return ShowQuestion("Really Closer Look");
            if (lastChoice == 1) goto Break;
            yield return ShowQuestion("Really Really Closer Look");
            if (lastChoice == 1) goto Break;
            yield return ShowQuestion("Really Really Really Closer Look");
            if (lastChoice == 1) goto Break;
            AudioManager.StopMusic();
            wentDownTrapdoor = true;
            Break:;
        }
        if (wentDownTrapdoor) {
            AudioManager.StopAllAmbienceSources();
            yield return BackgroundManager.FadeOut();
            yield return ShowConversation("Hell Ending");
            AudioManager.PlayMusic(hellMusic);
            yield return BackgroundManager.FadeIn(hellCg);
            yield return ShowConversation("Hell Ending 2");
            AudioManager.StopAllAmbienceSources();
            // hide the character and swtich scenes
            BackgroundManager.HideCharacter();
            yield return Exit();
            yield break;
        } else {
            yield return SneaksRoom();
        }
    }

    IEnumerator SneaksRoom() {
        // go to his room and either get trapped forever because its so cool, or crush yourself with an anvil
        yield return BackgroundManager.SlideIn(sneaks);
        yield return ShowConversation("Go Upstairs");
        yield return BackgroundManager.FadeOut();
        yield return ShowConversation("Waiting Outside Room");
        yield return ShowQuestion(purchasedAnvil && purchasedChalk ? "Peek Anvil" : "Peek");
        if (lastChoice == 2) {
            // only possible from peek anvil variant
            // player crushes themself and the game ends
            yield return ShowConversation("Anvil Trap");
            yield return BackgroundManager.FadeIn(sneaksRoom);
            yield return ShowConversation("Anvil Trap 2");
            AudioManager.PlaySound(metalPipe);
            yield return ShowConversation("Anvil Trap 3");
            AudioManager.PlaySound(metalPipe);
            yield return BackgroundManager.FadeOut();
            BackgroundManager.HideCharacter();
            yield return ShowConversation("Anvil Trap 4");
            yield return Exit();
            yield break;
        }
        BackgroundManager.HideCharacter();
        if (lastChoice == 0) {
            yield return ShowConversation("Peek");
            yield return BackgroundManager.FadeIn(peek1);
            yield return ShowConversation("Peek 2");
            BackgroundManager.UpdateBackground(peek2);
            yield return ShowConversation("Peek 3");
            yield return BackgroundManager.FadeOut();
            yield return ShowConversation("Peek 4");
        } else {
            yield return ShowConversation("Dont Peek");
        }
        yield return ShowConversation("Sneaks Ready");
        yield return BackgroundManager.FadeIn(sneaksRoomCg);
        yield return ShowConversation("Miku Room");
        yield return BackgroundManager.FadeOut();
        AudioManager.StopMusic();
        AudioManager.StopAllAmbienceSources();
        yield return ShowConversation("Miku Room 2");
        yield return ShowConversation("Miku Room 3");
        yield return new WaitForSeconds(2);
        yield return ShowConversation("Miku Room 4");
        yield return new WaitForSeconds(2);
        if (purchasedAnvil) {
            yield return ShowConversation("Kib Ending");
            yield return new WaitForSeconds(3);
        }
        yield return Exit();
    }

    IEnumerator Exit() {
        yield return new WaitUntil(() => !BackgroundManager.IsFading());
        SceneManager.LoadScene("Main Menu");
    }
}
