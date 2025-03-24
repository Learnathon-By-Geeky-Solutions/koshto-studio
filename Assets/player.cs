using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerControls controls;
    private float moveInputX;
    private bool isJumping, isDashing, isSprinting, isWallSliding;
    private bool facingRight = true;

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
    public bool isTouchingWall;

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
        controls = new PlayerControls();

        // Read only the X-axis value for movement
        controls.Gameplay.Move.performed += ctx => moveInputX = ctx.ReadValue<Vector2>().x;
        controls.Gameplay.Move.canceled += ctx => moveInputX = 0f;

        // Jump, Dash, and Sprint Events
        controls.Gameplay.Jump.performed += ctx => HandleJump();
        controls.Gameplay.Dash.performed += ctx => StartCoroutine(Dash());
        controls.Gameplay.Sprint.performed += ctx => isSprinting = true;
        controls.Gameplay.Sprint.canceled += ctx => isSprinting = false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movementSpeed = maxSpeed;
    }

    private void Update()
    {
        // ✅ Ground Check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // ✅ Coyote Time Fix
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            jumpCount = maxJumps;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // ✅ Sprinting
        movementSpeed = isSprinting ? sprintSpeed : maxSpeed;

        // ✅ Character Flip Fix
        if (moveInputX > 0 && !facingRight)
            Flip();
        else if (moveInputX < 0 && facingRight)
            Flip();

        // ✅ Wall Sliding Fix
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);
        if (isTouchingWall && !isGrounded && moveInputX != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void FixedUpdate()
    {
        // ✅ Movement with Acceleration
        float targetSpeed = moveInputX * movementSpeed;
        float speedDifference = targetSpeed - rb.velocity.x;
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.1f) ? acceleration : deceleration;
        float movementForce = speedDifference * accelerationRate;
        rb.AddForce(new Vector2(movementForce, 0), ForceMode2D.Force);
    }

    // 🚀 Handles Normal & Wall Jumping
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

    // 🧗 Fixed Wall Jump Code
    private void WallJump()
    {
        isWallSliding = false; // Disable sliding so jump isn't canceled

        // Determine jump direction (opposite of the wall)
        float jumpDirection = facingRight ? -1f : 1f;

        // ✅ Flip character if needed
        if ((jumpDirection > 0 && !facingRight) || (jumpDirection < 0 && facingRight))
        {
            Flip(); // Flip character so it faces the new direction
        }

        // Apply force for the wall jump
        rb.velocity = new Vector2(jumpDirection * wallJumpForce, jumpForce);

        // Prevent immediate movement input after jumping
        StartCoroutine(DisableMovementForWallJump());
    }

    // ⏳ Prevents instant wall re-stick
    private IEnumerator DisableMovementForWallJump()
    {
        moveInputX = 0; // Temporarily stop movement input
        yield return new WaitForSeconds(0.2f); // Small delay
    }

    // ⚡ Dash Function (Now Works Properly)
    private IEnumerator Dash()
    {
        isDashing = true;

        // Store original gravity
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0; // Disable gravity while dashing

        float dashDirection = facingRight ? 1f : -1f;

        // Apply instant dash velocity
        rb.velocity = new Vector2(dashDirection * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero;
        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    // 🔄 Flip Character Direction
    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void OnEnable() => controls.Gameplay.Enable();
    private void OnDisable() => controls.Gameplay.Disable();
}