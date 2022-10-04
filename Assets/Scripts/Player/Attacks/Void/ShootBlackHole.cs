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
    [SerializeField] GameObject blackHole;
    [SerializeField] GameObject blackHoleExplosion;
    [SerializeField] Transform bulletSpawner;
    [SerializeField] AudioClip blackHoleShoot;
    GameObject newBlackHole;
    Rigidbody2D newBlackHoleRB;
    Rigidbody2D playerRigidbody;
    GlobalAudio globalAudio;
    Animator animator;
    List<Rigidbody2D> nearbyEntities = new List<Rigidbody2D>();
    bool onCD = false;

    private void Start() {
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        globalAudio = FindObjectOfType<GlobalAudio>();
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
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            if (Vector2.Distance(newBlackHole.transform.position, mousePos) > 0.1f) 
            {
            newBlackHoleRB.velocity = new Vector2(mousePos.x - newBlackHole.transform.position.x, mousePos.y - newBlackHole.transform.position.y).normalized * projectileSpeed;
            }
            else newBlackHoleRB.velocity = Vector2.zero;
        }
    }

    void OnFire(InputValue value)
    {
        if (onCD || PlayerMovement.isDead || inAirShots >= inAirShotLimit) return;
        timeOfCast = Time.time;
        globalAudio.PlaySFX(blackHoleShoot, 0.5f);
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
        newBlackHole.GetComponent<Animator>().SetTrigger("Explode");
        if (Time.time - timeOfCast < 0.2f) {
            animator.SetTrigger("Jump");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0;
            PlayerMovement.isBeingPulled = true;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce((mousePos - transform.position).normalized * thrust, ForceMode2D.Impulse);
        }
        else {
            foreach (Rigidbody2D entity in nearbyEntities)
            {
                if (entity.gameObject.tag == "Player") PlayerMovement.isBeingPulled = true;
                else entity.GetComponent<EnemyMovement>().isBeingPulled = true;
                entity.AddForce((newBlackHole.transform.position - entity.transform.position).normalized * Mathf.Max(thrust, (1/ thrust / Vector2.Distance(newBlackHole.transform.position, entity.transform.position))), ForceMode2D.Impulse);

            }
        }

        onCD = true;
        inAirShots++;
        GameObject explosion = Instantiate(blackHoleExplosion, newBlackHole.transform.position, Quaternion.identity);
        StartCoroutine(ShrinkBlackhole(explosion));
        Destroy(newBlackHole);
        StartCoroutine(ShotCooldown());
    }

    public void UpdateNearbyEntities(Rigidbody2D entity, bool isAdding)
    {
        if (isAdding) nearbyEntities.Add(entity);
        else if (nearbyEntities.Contains(entity)) nearbyEntities.Remove(entity);
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

    IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(shotCD);
        onCD = false;
    }
}
