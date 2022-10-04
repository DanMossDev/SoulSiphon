using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHandler : MonoBehaviour
{
    [SerializeField] int enemyHP = 3;
    [SerializeField] float iTime = 0.15f;
    [SerializeField] AudioClip[] enemyDamage;
    SpriteRenderer spriteRenderer;
    EnemyMovement enemyMovement;
    GlobalAudio globalAudio;
    bool isInvincible = false;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyMovement = GetComponent<EnemyMovement>();
        globalAudio = FindObjectOfType<GlobalAudio>();
    }

    public void TakeDamage(int damage, int direction = 0)
    {
        if (isInvincible) return;
        globalAudio.PlaySFX(enemyDamage);
        enemyMovement.knockBack(direction);
        enemyHP += damage;
        if (enemyHP <= 0) enemyMovement.Die(direction);
        isInvincible = true;
        StartCoroutine(InvincibilityFrames());
    }

    IEnumerator InvincibilityFrames()
    {
        yield return new WaitForSeconds(iTime);
        isInvincible = false;
        spriteRenderer.color = Color.white;
    }
}
