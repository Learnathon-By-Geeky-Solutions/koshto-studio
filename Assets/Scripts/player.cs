using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerControls controls;

    // Input Variables
    private float moveInputX;
    private bool isJumping, isDashing, isSprinting, isWallSliding, isFacingRight = true;

    [Header("Movement Settings")]
    public float maxSpeed = 5f;
    public float acceleration = 8f;
    public float deceleration = 6f;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float coyoteTime = 0.1f;
    public float lowJumpMultiplier = 2f;
    public float fallMultiplier = 2.5f;
    private float coyoteTimeCounter;
    private bool isGrounded;
    private int jumpCount;
    public int maxJumps = 2;

    [Header("Wall Jump & Slide")]
    public float wallSlideSpeed = 2f;
    public float wallJumpForce = 10f;
    public Transform wallCheck;
    private bool isTouchingWall;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;

    [Header("Sprint Settings")]
    public float sprintSpeed = 8f;
    private float movementSpeed;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controls = new PlayerControls();

        // Input Events
        controls.Gameplay.Move.performed += ctx => moveInputX = ctx.ReadValue<Vector2>().x;
        controls.Gameplay.Move.canceled += ctx => moveInputX = 0f;
        controls.Gameplay.Jump.performed += ctx => HandleJump();
        controls.Gameplay.Dash.performed += ctx => StartCoroutine(Dash());
        controls.Gameplay.Sprint.performed += ctx => isSprinting = true;
        controls.Gameplay.Sprint.canceled += ctx => isSprinting = false;
    }

    private void Start()
    {
        movementSpeed = maxSpeed;
    }

    private void Update()
    {
        CheckGroundStatus();
        HandleMovement();
        HandleWallSlide();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    // ✅ Check Ground & Coyote Time
    private void CheckGroundStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            jumpCount = maxJumps;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    // ✅ Handles Movement Input & Character Flip
    private void HandleMovement()
    {
        movementSpeed = isSprinting ? sprintSpeed : maxSpeed;

        if (moveInputX > 0 && !isFacingRight)
            Flip();
        else if (moveInputX < 0 && isFacingRight)
            Flip();
    }

    // ✅ Handles Smooth Movement with Acceleration
    private void ApplyMovement()
    {
        float targetSpeed = moveInputX * movementSpeed;
        float speedDifference = targetSpeed - rb.velocity.x;
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.1f) ? acceleration : deceleration;
        rb.AddForce(new Vector2(speedDifference * accelerationRate, 0), ForceMode2D.Force);
    }

    // ✅ Handles Normal & Wall Jumping
    private void HandleJump()
    {
        if (isWallSliding)
        {
            WallJump();
        }
        else if (isGrounded || coyoteTimeCounter > 0 || jumpCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            coyoteTimeCounter = 0f;
            jumpCount--;
        }
    }

    // ✅ Wall Sliding Logic
    private void HandleWallSlide()
    {
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);
        isWallSliding = isTouchingWall && !isGrounded && moveInputX != 0;

        if (isWallSliding)
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
    }

    // ✅ Wall Jump with Proper Gravity Reset
    private void WallJump()
    {
        isWallSliding = false;
        rb.gravityScale = fallMultiplier;

        float jumpDirection = isFacingRight ? -1f : 1f;

        if ((jumpDirection > 0 && !isFacingRight) || (jumpDirection < 0 && isFacingRight))
        {
            Flip();
        }
        rb.velocity = new Vector2(jumpDirection * wallJumpForce, jumpForce);
        StartCoroutine(DisableMovementForWallJump());
    }

    private IEnumerator DisableMovementForWallJump()
    {
        float storedInput = moveInputX;
        moveInputX = 0;
        yield return new WaitForSeconds(0.2f);
        moveInputX = controls.Gameplay.Move.ReadValue<Vector2>().x;
    }

    // ✅ Dash Function with Proper Gravity Reset
    private IEnumerator Dash()
    {
        if (isDashing) yield break;

        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        float dashDirection = isFacingRight ? 1f : -1f;
        rb.velocity = new Vector2(dashDirection * dashSpeed, 0f);

        animator.SetTrigger("Dash");

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        rb.velocity = new Vector2(0, rb.velocity.y);
        isDashing = false;
    }

    // ✅ Flip Character Direction
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    // ✅ Update Animator Parameters
    private void UpdateAnimations()
    {
        animator.SetBool("isRunning", Mathf.Abs(moveInputX) > 0.1f);
        animator.SetBool("isJumping", !isGrounded && rb.velocity.y > 0);
        animator.SetBool("isFalling", !isGrounded && rb.velocity.y < 0);
        animator.SetBool("isWallSliding", isWallSliding);
    }

    private void OnEnable() => controls.Gameplay.Enable();
    private void OnDisable() => controls.Gameplay.Disable();
}
