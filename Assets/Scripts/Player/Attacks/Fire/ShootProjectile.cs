using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(LineRenderer))]
public class ShootProjectile : MonoBehaviour
{
    [Header("Game Feel")]
    [SerializeField] float maxProjectileSpeed = 5f;
    [SerializeField] float projectileChargeSpeed = 3f;
    [SerializeField] float shotCD = 0.5f;
    [SerializeField] float turnRate = 150;
    [Space]
    [Header("Prefabs and Game Objects")]
    [SerializeField] GameObject projectile;
    [SerializeField] Animator animator;
    Transform projectileSpawner;
    [Space]
    [Header("Audio")]
    [SerializeField] AudioClip[] chargeFire;
    [SerializeField] AudioClip[] shootFire;

    //Used for logic
    float projectileSpeed = 1f;
    bool onCD = false;
    bool isShooting = false;
    float angle;
    //Cached references
    LineRenderer lineRenderer;


    void OnEnable() 
    {
        lineRenderer = GetComponent<LineRenderer>();
        projectileSpawner = transform.parent;
    }


    void FixedUpdate() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        angle = Mathf.Rad2Deg * Mathf.Atan2(projectileSpawner.position.x - mousePos.x, mousePos.y - projectileSpawner.position.y);
        projectileSpawner.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (isShooting) {
            if (PlayerMovement.isDead) {lineRenderer.positionCount = 0; return;}

            Vector2 projectileVelocity = new Vector2(mousePos.x - projectileSpawner.position.x, mousePos.y - projectileSpawner.position.y).normalized * projectileSpeed;
            Vector3[] trajectory = Plot(projectile.GetComponent<Rigidbody2D>(), projectileSpawner.position, projectileVelocity, 2000);
            lineRenderer.SetPositions(trajectory);

            if (projectileSpeed == maxProjectileSpeed) return;
            projectileSpeed += Time.deltaTime * projectileChargeSpeed;
            if (projectileSpeed > maxProjectileSpeed) projectileSpeed = maxProjectileSpeed;
        }
    }

    public void OnFire()
    {
        if (PlayerMovement.isDead || onCD) return;

        projectileSpeed = 1;
        lineRenderer.positionCount = 2000;

        animator.ResetTrigger("Release");
        animator.SetTrigger("Charge");
        animator.SetBool("isAttacking", true);

        isShooting = true;
    }

    void OnReleaseFire()
    {
        lineRenderer.positionCount = 0;

        if (PlayerMovement.isDead || onCD || !isShooting) return;

        GameObject newProjectile = Instantiate(projectile, projectileSpawner.position, Quaternion.Euler(0, 0, angle + 90));

        onCD = true;
        isShooting = false;

        animator.SetTrigger("Release");
        animator.SetBool("isAttacking", false);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Rigidbody2D projectileRB = newProjectile.GetComponent<Rigidbody2D>();
        projectileRB.velocity = new Vector2(mousePos.x - projectileSpawner.position.x, mousePos.y - projectileSpawner.position.y).normalized * projectileSpeed;
        projectileSpeed = 1;

        StartCoroutine(RotateProjectile(newProjectile, projectileRB));
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

    IEnumerator RotateProjectile(GameObject projectile, Rigidbody2D projectileRB)
    {
        while (projectile != null)
        {
            yield return new WaitForSecondsRealtime(0.02f);
            if (projectile == null) yield break;
            float angle = Mathf.Atan2(projectileRB.velocity.y, projectileRB.velocity.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        }
    }

    IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(shotCD);
        onCD = false;
    }
}
