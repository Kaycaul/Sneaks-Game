using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour {

    [SerializeField] GameObject backgroundObjectPrefab;

    public static BackgroundManager instance;

    Image background;

    // create a new background image with the provided sprite
    public static void UpdateBackground(Sprite sprite) {
        instance.background.sprite = sprite;
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        GameObject backgroundObject = Instantiate(backgroundObjectPrefab);    
        backgroundObject.transform.SetParent(transform);
        background = backgroundObject.transform.GetChild(0).GetComponent<Image>();
    }

}
    

