using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class ShootBlackHole : MonoBehaviour
{
    [Space]
    [Header("Projectile Settings")]
    [SerializeField] float shotCD = 0.5f;
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float thrust = 10f;
    [SerializeField] int inAirShotLimit = 2;
    float timeOfCast;
    int inAirShots = 0;

    [Space]
    [Header("Prefabs/Transforms")]
    [SerializeField] GameObject blackHole;
    [SerializeField] GameObject blackHoleExplosion;
    [SerializeField] GameObject voidSlash;
    [SerializeField] GameObject player;

    [Space]
    [Header("Audio")]
    [SerializeField] AudioClip[] blackHoleCharge;
    [SerializeField] AudioClip[] blackHoleExplode;
    Transform bulletSpawner;
    GameObject newBlackHole;
    Rigidbody2D newBlackHoleRB;
    Rigidbody2D playerRigidbody;
    CapsuleCollider2D playerCollider;
    AudioController audioController;
    Animator animator;
    bool onCD = false;

    private void Start() {
        animator = player.GetComponent<Animator>();
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        playerCollider = player.GetComponent<CapsuleCollider2D>();
        audioController = GetComponentInParent<AudioController>();
        bulletSpawner = transform.parent;
    }

    void Update() {
        if (animator.GetBool("Grounded")) inAirShots = 0;
    }

    void FixedUpdate() {
        if (newBlackHole && newBlackHole.transform.localScale.x < 2) {
            newBlackHole.transform.localScale *= 1.05f;
            newBlackHole.GetComponentInChildren<Light2D>().pointLightOuterRadius = newBlackHole.transform.localScale.x * 2;
            newBlackHole.GetComponentInChildren<Light2D>().pointLightInnerRadius = newBlackHole.transform.localScale.x * 2;
        }
        if (newBlackHoleRB)
        {
            if (!Mouse.current.rightButton.IsPressed()) OnReleaseFire();
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            if (Vector2.Distance(newBlackHole.transform.position, mousePos) > 0.1f) 
            {
            newBlackHoleRB.velocity = new Vector2(mousePos.x - newBlackHole.transform.position.x, mousePos.y - newBlackHole.transform.position.y).normalized * projectileSpeed;
            }
            else newBlackHoleRB.velocity = Vector2.zero;


            Collider2D[] nearbyEntities = Physics2D.OverlapCircleAll(newBlackHoleRB.position, newBlackHole.transform.localScale.x * 2, LayerMask.GetMask("Enemies"));
            foreach (Collider2D e in nearbyEntities)
            {
                Rigidbody2D entity = e.GetComponent<Rigidbody2D>();
                entity.GetComponent<EnemyMovement>().isBeingPulled = true;
                entity.AddForce((newBlackHole.transform.position - entity.transform.position).normalized * Mathf.Max(thrust, (1 / thrust / Vector2.Distance(newBlackHole.transform.position, entity.transform.position))), ForceMode2D.Force);
            }
        }
    }

    void OnFire(InputValue value)
    {
        if (onCD || PlayerMovement.isDead || inAirShots >= inAirShotLimit) return;
        timeOfCast = Time.time;
        audioController.PlaySFX(blackHoleCharge, 0.5f, true);
        animator.ResetTrigger("Release");
        animator.SetTrigger("Charge");
        animator.SetBool("isAttacking", true);
        newBlackHole = Instantiate(blackHole, bulletSpawner.position, Quaternion.identity);
        newBlackHoleRB = newBlackHole.GetComponent<Rigidbody2D>();
    }

    public void OnReleaseFire()
    {
        if (onCD || inAirShots >= inAirShotLimit || !newBlackHole) return;
        animator.SetTrigger("Release");
        animator.SetBool("isAttacking", false);
        audioController.StopSFX();
        audioController.PlaySFX(blackHoleExplode);
        newBlackHole.GetComponent<Animator>().SetTrigger("Explode");
        if (Time.time - timeOfCast < 0.2f) {
            animator.SetTrigger("Jump");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0;
            PlayerMovement.isBeingPulled = true;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce((mousePos - player.transform.position).normalized * thrust, ForceMode2D.Impulse);
        }
        else
        {
            Collider2D[] nearbyPlayer = Physics2D.OverlapCircleAll(newBlackHoleRB.position, newBlackHole.transform.localScale.x * 2, LayerMask.GetMask("Player"));
            Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(newBlackHoleRB.position, newBlackHole.transform.localScale.x * 2, LayerMask.GetMask("Enemies"));
            if (nearbyPlayer.Length != 0 && nearbyEnemies.Length == 0) 
            {
                PlayerMovement.isBeingPulled = true;
                playerRigidbody.AddForce((newBlackHole.transform.position - player.transform.position).normalized * Mathf.Max(thrust, (1/ thrust / Vector2.Distance(newBlackHole.transform.position, player.transform.position))), ForceMode2D.Impulse);
            }
            else if (nearbyPlayer.Length != 0 && nearbyEnemies.Length != 0)
            {
                Vector3 blackHolePos = newBlackHole.transform.position;
                player.transform.position += new Vector3(blackHolePos.x - player.transform.position.x, blackHolePos.y - player.transform.position.y, 0) * 2;
                float gravScale = playerRigidbody.gravityScale;
                playerRigidbody.gravityScale = 0;
                PlayerMovement.hitStunned = true;
                playerRigidbody.velocity = Vector2.zero;
                StartCoroutine(RestoreGravity(gravScale));
                foreach (Collider2D enemy in nearbyEnemies)
                {
                    enemy.GetComponent<EnemyDamageHandler>().TakeDamage(-1, 0);
                    GameObject slash = Instantiate(voidSlash, enemy.transform);
                    Destroy(slash, 1);
                }
            }
        }

        onCD = true;
        inAirShots++;
        GameObject explosion = Instantiate(blackHoleExplosion, newBlackHole.transform.position, Quaternion.identity);
        StartCoroutine(ShrinkBlackhole(explosion));
        Destroy(newBlackHole);
        StartCoroutine(ShotCooldown());
    }

    IEnumerator ShrinkBlackhole(GameObject explosion)
    {
        while (explosion.transform.localScale.x > 0.1)
        {
            yield return new WaitForSeconds(0.02f);
            explosion.transform.localScale *= 0.9f;
        }
        Destroy(explosion);
    }

    IEnumerator RestoreGravity(float scale)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        PlayerMovement.hitStunned = false;
        playerRigidbody.gravityScale = scale;
    }

    IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(shotCD);
        onCD = false;
    }
}
