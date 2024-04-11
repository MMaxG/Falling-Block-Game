using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    float speed = 7;
    float screenHalfWidthInGameUnits;
    float halfPlayerWidth;
    public event System.Action OnPlayerDeath;

    void Start() {
        halfPlayerWidth = transform.localScale.x / 2f;
        screenHalfWidthInGameUnits = Camera.main.aspect * Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void Update() {

        float inputX = Input.GetAxisRaw("Horizontal");
        float velocity = inputX * speed;
        transform.Translate(Vector2.right * velocity * Time.deltaTime);

        if (transform.position.x < -screenHalfWidthInGameUnits - halfPlayerWidth) {
            transform.position = new Vector2(screenHalfWidthInGameUnits + halfPlayerWidth, transform.position.y);
        }

        if (transform.position.x > screenHalfWidthInGameUnits + halfPlayerWidth) {
            transform.position = new Vector2(-screenHalfWidthInGameUnits-halfPlayerWidth, transform.position.y);
        }
    }

    void OnTriggerEnter2D(Collider2D triggerCollider) {
        if (triggerCollider.tag == "Falling Block") {
            if (OnPlayerDeath != null) {
                OnPlayerDeath();
            }
            Destroy (gameObject);
        }
    }
}
