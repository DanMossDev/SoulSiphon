using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    Rigidbody2D rigidBody;
    BoxCollider2D boxCollider;
    Animator animator;
    public bool isBeingPulled;
    bool hitStunned = false;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isBeingPulled) rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
    }

    void OnTriggerExit2D(Collider2D other) {
        if ((other.tag == "Ground")) turnAround();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if ((other.tag == "Enemy")) turnAround();
        if (other.tag == "Hazard") Die(0);
    }

    void turnAround()
    {
        moveSpeed *= -1;
        transform.localScale = new Vector2(transform.localScale.x * -1, 1f);
    }

    public void knockBack(int direction)
    {
        isBeingPulled = true;
        rigidBody.velocity = new Vector2(direction * 2, 1);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.tag == "Ground") isBeingPulled = false;
    }

    public void Die(int direction)
    {
        isBeingPulled = true;
        rigidBody.velocity = new Vector2(direction * 3, 5);
        animator.SetTrigger("Die");
        GetComponent<DealDamage>().SetDamage(0);
        Destroy(gameObject, 0.2f);
    }
}
