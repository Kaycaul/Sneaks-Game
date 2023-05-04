using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementTest : MonoBehaviour {
  
    [SerializeField]
    float speed = 48;
    [SerializeField]
    float drag = 6;
    [SerializeField]
    TextSpawner dialogueOnSpace;

    Rigidbody2D rb;
    bool talking = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        dialogueOnSpace.onFinished += DoneTalking;
    }

    // start or continue the text box on space
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!talking) {
                dialogueOnSpace.StartText();
                talking = true;
            } else {
                dialogueOnSpace.NextTextBox();
            }
        }
    }

    void DoneTalking() {
        talking = false;
    }

    void FixedUpdate() {
        // get a vector representing the player input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input.magnitude > 1) {
            input.Normalize();
        }
        // add force to the player if not talking
        if (!talking) rb.AddForce(input * speed); 
        // add drag
        rb.AddForce(-rb.velocity * drag);
    }
}