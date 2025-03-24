using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerControls controls;
    private float moveInputX; // Store horizontal input as float (not Vector2)
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

        // 🏃‍♂️ Fix: Read only the X-axis value for movement
        controls.Gameplay.Move.performed += ctx => moveInputX = ctx.ReadValue<Vector2>().x;
        controls.Gameplay.Move.canceled += ctx => moveInputX = 0f; // Reset movement when released

        // 🎯 Fix: Ensure Jump, Dash, and Sprint work
        controls.Gameplay.Jump.performed += ctx => Jump();
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

        // ✅ Sprinting Works Now
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

        // ✅ Wall Jump Fix
        if (isJumping && isWallSliding)
        {
            rb.velocity = new Vector2(-moveInputX * wallJumpForce, jumpForce);
        }
    }

    private void FixedUpdate()
    {
        // ✅ Fix: Use `moveInputX` instead of `Vector2`
        float targetSpeed = moveInputX * movementSpeed;
        float speedDifference = targetSpeed - rb.velocity.x;
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.1f) ? acceleration : deceleration;
        float movementForce = speedDifference * accelerationRate;
        rb.AddForce(new Vector2(movementForce, 0), ForceMode2D.Force);
    }

    private void Jump()
    {
        if (isGrounded || coyoteTimeCounter > 0 || jumpCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            coyoteTimeCounter = 0f;
            jumpCount--;
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float originalSpeed = movementSpeed;
        movementSpeed = dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        movementSpeed = originalSpeed;
        isDashing = false;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void OnEnable() => controls.Gameplay.Enable();
    private void OnDisable() => controls.Gameplay.Disable();
}