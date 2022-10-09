using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectiles : MonoBehaviour
{
    [Header("Bullet Logic")]
    [SerializeField] float bulletLife = 0;
    [SerializeField] float damageMultiplier = 1;
    [Space]
    [Header("Audio")]
    [SerializeField] AudioClip[] bulletImpact;

    void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.tag == "Enemy") other.collider.GetComponent<EnemyDamageHandler>().TakeDamage((int)Mathf.Round(-PlayerStats.projectileDamage * damageMultiplier), new Vector2(other.collider.transform.position.x - transform.position.x, other.collider.transform.position.y - transform.position.y).normalized);
        GlobalAudio.PlaySFX(bulletImpact);
    }
}