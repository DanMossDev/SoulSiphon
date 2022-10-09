using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageHandler : MonoBehaviour
{
    [Header("Game Feel")]
    [SerializeField] float invinciblilityTime = 1f;
    [SerializeField] float hitStunTime = 0.25f;
    [Space]
    [Header("Audio")]
    [SerializeField] AudioClip[] playerDamage;

    //Cached referenced
    Animator animator;
    Rigidbody2D rigidBody;

    bool isInvincible = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.tag == "Enemy" && !isInvincible)
        {
            DealDamage dealDamage = other.collider.GetComponent<DealDamage>();
            Vector2 normal = other.contacts[0].normal;
            DamageBoost(normal);
            TakeDamage(-dealDamage.damage);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard") && !isInvincible)
        {
            TakeDamage(-1);
            DamageBoost(Vector2.up);
        }
    }

    public void TakeDamage(int damage)
    {
        GlobalAudio.PlaySFX(playerDamage);
        animator.SetTrigger("Hurt");
        PlayerStats.currentHP += damage;
        if (PlayerStats.currentHP <= 0) {
            Die();
            PlayerStats.InitHP();
        }
        isInvincible = true;
        StartCoroutine(InvincibilityFrames());
    }

    public void DamageBoost(Vector2 normal)
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        rigidBody.velocity += (normal + Vector2.up) * 7;
        PlayerMovement.hitStunned = true;
        StartCoroutine(HitStun(false));
    }

    public void Die() 
    {
        animator.SetTrigger("Dying");
        PlayerMovement.isDead = true;
        PhysicsMaterial2D deathMaterial = new PhysicsMaterial2D();
        deathMaterial.friction = 1000;
        rigidBody.sharedMaterial = deathMaterial;
        GetComponent<CoinCounter>().OnDeath();
        StartCoroutine(HitStun(true));
    }

    IEnumerator HitStun(bool isDying)
    {
        if (isDying) {
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        yield return new WaitForSeconds(hitStunTime);
        PlayerMovement.hitStunned = false;
    }

    IEnumerator InvincibilityFrames()
    {
        yield return new WaitForSeconds(invinciblilityTime);
        isInvincible = false;
    }
}
