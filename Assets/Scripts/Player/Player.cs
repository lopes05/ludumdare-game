using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed;
    Animator anim;

    void Awake() {
        this.anim = GetComponent<Animator>();
    }

    void Update() {
        var movement = Move();
        Debug.Log(movement);

        SetDirection(movement);
    }

    float Move() {
        var speedX = Input.GetAxis("Horizontal");
        var speedY = Input.GetAxis("Vertical");

        var direction = new Vector3(speedX, speedY);
        if(direction.magnitude > 1) {
            direction.Normalize();
        }
        direction = new Vector3(direction.x * speed, direction.y*speed, 0);
        transform.position += direction;

        return direction.magnitude;
    }

    void SetDirection(float movement) {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var distance = (worldPosition - transform.position);

        if(distance.magnitude > 1) {
            distance.Normalize();
        }

        this.anim.SetFloat("PositionX", distance.x);
        this.anim.SetFloat("PositionY", distance.y);

        this.anim.SetBool("Moving", movement != 0);
    }
}
