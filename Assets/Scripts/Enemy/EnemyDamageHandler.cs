using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHandler : MonoBehaviour
{
    [SerializeField] int enemyHP = 3;
    [SerializeField] float iTime = 0.15f;
    [SerializeField] AudioClip[] enemyDamage;
    Rigidbody2D rigidBody;
    Animator animator;
    SpriteRenderer spriteRenderer;
    bool isInvincible = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int damage, Vector2 direction)
    {
        if (isInvincible) return;
        GlobalAudio.PlaySFX(enemyDamage);
        knockBack(direction);
        enemyHP += damage;
        if (enemyHP <= 0) Die(direction);
        isInvincible = true;
        StartCoroutine(InvincibilityFrames());
    }

    void knockBack(Vector2 direction)
    {
        //isBeingPulled = true;
        rigidBody.velocity = direction * 2;
    }

    void Die(Vector2 direction)
    {
        // isBeingPulled = true;
        rigidBody.velocity = direction * 3;
        animator.SetTrigger("Die");
        GetComponent<DealDamage>().SetDamage(0);
        Destroy(gameObject, 0.2f);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Hazard") Die(Vector2.zero);
    }

    IEnumerator InvincibilityFrames()
    {
        yield return new WaitForSeconds(iTime);
        isInvincible = false;
    }
}
