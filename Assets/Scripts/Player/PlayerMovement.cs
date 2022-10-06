using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Basic Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float coyoteTime = 0.05f;

    float lastPressedJump;
    float lastGroundedTime;
    float timeOfJump;

    [Space]
    [Header("Audio")]
    [SerializeField] AudioClip[] jump;

    //Used for movement logic
    [HideInInspector] public static bool isDead = false; 
    [HideInInspector] public static bool hitStunned = false;
    [HideInInspector] public static bool isBeingPulled = false;
    Vector2 moveInput;

    //Cached references
    [HideInInspector] public Rigidbody2D rigidBody;
    Animator animator;
    BoxCollider2D boxCollider;
    CapsuleCollider2D capsuleCollider;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponentInChildren<BoxCollider2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        isDead = false;
        hitStunned = false;
    }


    void Update()
    {
        if (isDead || hitStunned) return;
        FlipSprite();
        if (boxCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            animator.SetBool("Grounded", true);
            if (isBeingPulled) StartCoroutine(CheckBeingPulled());
            lastGroundedTime = Time.time;
            if (Time.time - lastPressedJump <= coyoteTime) OnJump(new InputValue());
        } else {
            animator.SetBool("Grounded", false);
            animator.SetFloat("AirSpeedY", rigidBody.velocity.y);
        }
    }

    void FixedUpdate() {
        if (isDead || hitStunned) return;
        Run();
    }


    void OnMove(InputValue value)
    {
        if (isDead) return;
        moveInput = value.Get<Vector2>();
        // animator.SetBool("isLookingUp", moveInput.y > Mathf.Epsilon);
        // animator.SetBool("isLookingDown", moveInput.y < -Mathf.Epsilon);
    }

    void OnJump(InputValue value)
    {
        if (isDead || Time.time - timeOfJump < 0.2f || isBeingPulled) return;
        lastPressedJump = Time.time;
        if (Time.time - lastGroundedTime <= coyoteTime) {
            GlobalAudio.PlaySFX(jump);
            animator.SetTrigger("Jump");
            rigidBody.velocity += Vector2.up * jumpHeight;
            timeOfJump = Time.time;
        }
    }


    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rigidBody.velocity.y);
        if (isBeingPulled) rigidBody.AddForce(new Vector2(playerVelocity.x * 2, 0));
        else rigidBody.velocity = playerVelocity;
    }

    void FlipSprite() 
    {
        if (animator.GetBool("isAttacking")) {
            transform.localScale = new Vector2(Mathf.Sign(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()).x - transform.position.x), 1f);
            return;
        }

        if (Mathf.Abs(rigidBody.velocity.x) > 0.01)
        {
            animator.SetBool("isRunning", true);
            transform.localScale = new Vector2(Mathf.Sign(rigidBody.velocity.x), 1f);
        }
        else animator.SetBool("isRunning", false);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.tag == "Ground") isBeingPulled = false;
    }

    IEnumerator CheckBeingPulled()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        if (isBeingPulled && boxCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            while (Mathf.Abs(rigidBody.velocity.x) > 0.05)
            {
                rigidBody.velocity *= 0.99f;
                yield return new WaitForFixedUpdate();
            }
            rigidBody.velocity = Vector2.zero;
            isBeingPulled = false;
        }
    }
}
