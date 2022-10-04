using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootFireball : MonoBehaviour
{
    [Header("Game Feel")]
    [SerializeField] float maxBulletSpeed = 5f;
    [SerializeField] float bulletChargeSpeed = 3f;
    [SerializeField] float shotCD = 0.5f;
    [SerializeField] float turnRate = 150;
    [Space]
    [Header("Prefabs and Game Objects")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawner;
    [Space]
    [Header("Audio")]
    [SerializeField] AudioClip drawBow;
    [SerializeField] AudioClip releaseBow;

    //Used for logic
    float bulletSpeed = 1f;
    bool onCD = false;
    bool isShooting = false;
    float angle;
    //Cached references
    Animator animator;
    LineRenderer lineRenderer;
    AudioController audioController;

    void Start() 
    {
        animator = GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();
        audioController = GetComponentInChildren<AudioController>();
    }


    void FixedUpdate() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        angle = Mathf.Rad2Deg * Mathf.Atan2(bulletSpawner.position.x - mousePos.x, mousePos.y - bulletSpawner.position.y);
        bulletSpawner.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (isShooting) {
            if (PlayerMovement.isDead) {lineRenderer.positionCount = 0; return;}
            Vector2 bulletVelocity = new Vector2(mousePos.x - bulletSpawner.position.x, mousePos.y - bulletSpawner.position.y).normalized * bulletSpeed;
            Vector3[] trajectory = Plot(bullet.GetComponent<Rigidbody2D>(), bulletSpawner.position, bulletVelocity, 2000);
            lineRenderer.SetPositions(trajectory);

            if (bulletSpeed == maxBulletSpeed) return;
            bulletSpeed += Time.deltaTime * bulletChargeSpeed;
            if (bulletSpeed > maxBulletSpeed) bulletSpeed = maxBulletSpeed;

        }
    }

    void OnFire(InputValue value)
    {
        if (PlayerMovement.isDead || onCD) return;
        bulletSpeed = 0;
        lineRenderer.positionCount = 2000;
        audioController.PlaySFX(drawBow);
        animator.ResetTrigger("Release");
        animator.SetTrigger("Charge");
        animator.SetBool("isAttacking", true);
        isShooting = true;
    }

    void OnReleaseFire(InputValue value)
    {
        lineRenderer.positionCount = 0;
        if (PlayerMovement.isDead || onCD || !isShooting) return;
        GameObject newBullet = Instantiate(bullet, bulletSpawner.position, Quaternion.Euler(0, 0, angle + 90));
        onCD = true;
        isShooting = false;
        animator.SetTrigger("Release");
        animator.SetBool("isAttacking", false);
        audioController.StopSFX();
        audioController.PlaySFX(releaseBow);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Rigidbody2D bulletRB = newBullet.GetComponent<Rigidbody2D>();
        bulletRB.velocity = new Vector2(mousePos.x - bulletSpawner.position.x, mousePos.y - bulletSpawner.position.y).normalized * bulletSpeed;
        bulletSpeed = 1f;
        StartCoroutine(RotateBullet(newBullet, bulletRB));
        StartCoroutine(ShotCooldown());
    }

    Vector3[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector3[] results = new Vector3[steps];

        float timestep = Time.deltaTime / Physics2D.velocityIterations;
        Vector2 gravityAcceleration = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;
        float drag = 1f - timestep * rigidbody.drag;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAcceleration;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }

        return results;
    }

    IEnumerator RotateBullet(GameObject bullet, Rigidbody2D bulletRB)
    {
        while (bullet != null)
        {
            yield return new WaitForSecondsRealtime(0.02f);
            if (bullet == null) yield break;
            float angle = Mathf.Atan2(bulletRB.velocity.y, bulletRB.velocity.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        }
    }

    IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(shotCD);
        onCD = false;
    }
}
