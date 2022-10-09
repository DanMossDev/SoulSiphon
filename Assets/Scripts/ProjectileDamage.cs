using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    [Header("Bullet Logic")]
    public int bulletDamage = 1;
    [SerializeField] float bulletLife = 0;

    [Space]
    [Header("Audio")]
    [SerializeField] AudioClip[] bulletImpact;


    void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.tag == "Player") other.collider.GetComponent<DamageHandler>().TakeDamage(-bulletDamage);
        GlobalAudio.PlaySFX(bulletImpact);
    }
}
