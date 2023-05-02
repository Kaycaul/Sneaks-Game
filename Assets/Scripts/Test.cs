using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Test : MonoBehaviour {
  
    Rigidbody2D rb;

    [SerializeField]
    float speed = 48;
    [SerializeField]
    float topSpeed = 35;
    [SerializeField]
    float drag = 6;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    void FixedUpdate() {
        // get a vector representing the player input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input.magnitude > 1) {
            input.Normalize();
        }
        // add force to the player
        rb.AddForce(input * speed); 
        // add drag
        rb.AddForce(-rb.velocity * drag);
    }
}
