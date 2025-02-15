using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
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
    private bool isTouchingWall;
    private bool isWallSliding;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    private bool isDashing;

    [Header("Sprint Settings")]
    public float sprintSpeed = 8f;
    private float movementSpeed;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movementSpeed = maxSpeed;
    }

    void Update()
    {
        movement = Input.GetAxisRaw("Horizontal");

        // Ground Check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Coyote Time
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            jumpCount = maxJumps; // Reset jump count
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump Logic
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || coyoteTimeCounter > 0 || jumpCount > 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            coyoteTimeCounter = 0f; // Prevent multiple jumps using coyote time
            jumpCount--;
        }

        // Variable Jump Height
        if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        // Sprinting
        movementSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : maxSpeed;

        // Flip Character
        if (movement > 0 && !facingRight)
            Flip();
        else if (movement < 0 && facingRight)
            Flip();

        // Wall Sliding
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);
        if (isTouchingWall && !isGrounded && movement != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
        else
        {
            isWallSliding = false;
        }

        // Wall Jump
        if (Input.GetKeyDown(KeyCode.Space) && isWallSliding)
        {
            rb.velocity = new Vector2(-movement * wallJumpForce, jumpForce);
        }

        // Dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        // Smooth Movement
        float targetSpeed = movement * movementSpeed;
        float speedDifference = targetSpeed - rb.velocity.x;
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.1f) ? acceleration : deceleration;
        float movementForce = speedDifference * accelerationRate;
        rb.AddForce(new Vector2(movementForce, 0), ForceMode2D.Force);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
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
}