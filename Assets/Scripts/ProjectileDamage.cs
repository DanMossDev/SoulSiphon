using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    [Header("Bullet Logic")]
    public int bulletDamage = 1;
    [SerializeField] float bulletLife = 0;
    [SerializeField] bool playerBullet = false; //Stops player from damaging themself
    [Space]
    [Header("Audio")]
    [SerializeField] AudioClip bulletImpact;

    //Cached references
    GlobalAudio globalAudio;
    BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        globalAudio = FindObjectOfType<GlobalAudio>();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.tag == "Player" && playerBullet) return;
        else if (other.collider.tag == "Player") other.collider.GetComponent<DamageHandler>().TakeDamage(-bulletDamage);
        else if (other.collider.tag == "Enemy" && playerBullet) other.collider.GetComponent<EnemyDamageHandler>().TakeDamage(-bulletDamage, (int)Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x));
        else {boxCollider.enabled = false;}
        globalAudio.PlaySFX(bulletImpact);
        Destroy(this.gameObject, bulletLife);
    }
}
