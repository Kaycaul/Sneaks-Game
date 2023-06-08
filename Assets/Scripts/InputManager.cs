using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    static InputManager instance;

    public static event System.Action OnNextTextbox;
    public static event System.Action OnPause;

    private void Awake() {
        // delete or persist, there can be only one
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) OnPause?.Invoke();
        if (Input.GetKeyDown(KeyCode.Space) ||
        Input.GetKeyDown(KeyCode.Z) ||
        Input.GetKeyDown(KeyCode.X)) 
        OnNextTextbox?.Invoke();
    }
}
