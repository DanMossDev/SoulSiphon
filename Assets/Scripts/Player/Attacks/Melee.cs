using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Melee : MonoBehaviour
{
    [Space]
    [Header("Melee Options")]
    [SerializeField] int damage = 1;
    [SerializeField] float cd = 0.2f;
    [SerializeField] float attackRange;
    bool onCD = false;

    [Space]
    [Header("Prefabs/Layers")]
    [SerializeField] GameObject sword;
    [SerializeField] Transform swordSpawn;
    [SerializeField] Transform hitboxSpawn;
    [SerializeField] LayerMask enemyLayer;

    [Space]
    [Header("Audio")]
    [SerializeField] AudioClip swingSword;
    GlobalAudio globalAudio;
    Animator animator;
    
    void Start() {
        animator = GetComponent<Animator>();
        globalAudio = FindObjectOfType<GlobalAudio>();
    }
     void OnAttack(InputValue value)
    {
        if (PlayerMovement.isDead || onCD) return;
        globalAudio.PlaySFX(swingSword);
        animator.ResetTrigger("Release");
        animator.SetTrigger("Melee");
        onCD = true;

        GameObject newSword = Instantiate(sword, swordSpawn.position, Quaternion.identity, gameObject.transform);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitboxSpawn.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyDamageHandler>().TakeDamage(-damage, (int)Mathf.Sign(enemy.transform.position.x - transform.position.x));
            Rigidbody2D enemyRB = enemy.GetComponent<Rigidbody2D>();
        }
        Destroy(newSword, 0.15f);
        StartCoroutine(AttackCooldown());
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(hitboxSpawn.position, attackRange);
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(cd);

        onCD = false;
    }
}
