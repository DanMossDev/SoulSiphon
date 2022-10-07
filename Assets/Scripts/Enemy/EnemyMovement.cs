using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    Rigidbody2D rigidBody;
    BoxCollider2D boxCollider;
    Animator animator;
    EnemyState state;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        state = GetComponent<EnemyState>();
    }

    void Update()
    {
        if (!state.isBeingPulled) rigidBody.AddForce(new Vector2(moveSpeed, rigidBody.velocity.y));
    }

    void OnTriggerExit2D(Collider2D other) {
        if ((other.tag == "Ground")) turnAround();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if ((other.tag == "Enemy")) turnAround();
    }

    void turnAround()
    {
        if (state.isBeingPulled) return;
        rigidBody.velocity *= -1;
        moveSpeed *= -1;
        transform.localScale = new Vector2(transform.localScale.x * -1, 1f);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.tag == "Ground") state.isBeingPulled = false;
    }
}
