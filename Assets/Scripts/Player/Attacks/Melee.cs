using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Melee : MonoBehaviour
{
    [Space]
    [Header("Melee Options")]
    [SerializeField] float attackRange;
    bool onCD = false;
    float timeAttacked;

    [Space]
    [Header("Prefabs/Layers")]
    [SerializeField] Transform swordSpawn;
    [SerializeField] Transform hitboxSpawn;
    [SerializeField] LayerMask enemyLayer;

    [Space]
    [Header("Audio")]
    [SerializeField] AudioClip[] swingSword;
    Animator animator;
    
    void Start() 
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Time.time - timeAttacked < 0.15f) DetectEnemies();
    }
    void OnAttack(InputValue value)
    {
        if (PlayerMovement.isDead || onCD) return;
        GlobalAudio.PlaySFX(swingSword);
        animator.ResetTrigger("Release");
        animator.SetTrigger("Melee");
        onCD = true;

        timeAttacked = Time.time;
        StartCoroutine(AttackCooldown());
    }

    void DetectEnemies()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(hitboxSpawn.position, new Vector2(attackRange, 1f), 0, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyDamageHandler>().TakeDamage(-PlayerStats.meleeDamage, new Vector2(enemy.transform.position.x - transform.position.x, 1).normalized);
            Rigidbody2D enemyRB = enemy.GetComponent<Rigidbody2D>();
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(hitboxSpawn.position, new Vector2(attackRange, 1f));
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(PlayerStats.meleeAttackRate);
        onCD = false;
    }
}
