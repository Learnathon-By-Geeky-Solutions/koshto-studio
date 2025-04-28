using System.Collections;
using UnityEngine;
using Game;

namespace Player.Input
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputManager inputManager;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform wallCheck;
        [SerializeField] private LayerMask floorLayer;
        [SerializeField] private LayerMask wallLayer;

        [Header("Movement")]
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float sprintSpeed = 10f;
        [SerializeField] private float acceleration = 8f;
        [SerializeField] private float deceleration = 6f;

        [Header("Jump")]
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float coyoteTime = 0.1f;
        [SerializeField] private float fallMultiplier = 2.5f;
        [SerializeField] private int maxJumps = 2;

        [Header("Dash")]
        [SerializeField] private float dashSpeed = 20f;
        [SerializeField] private float dashDuration = 0.2f;

        [Header("Wall Jump & Slide")]
        [SerializeField] private float wallSlideSpeed = 2f;
        [SerializeField] private float wallJumpForce = 10f;
        [SerializeField] private float wallJumpDuration = 0.2f;
        private Rigidbody2D rb;
        private Animator animator;

        private float moveInputX;
        private float coyoteTimeCounter;
        private int jumpCount;

        private bool isFacingRight = true;
        private bool isGrounded;
        private bool isTouchingWall;
        private bool isWallSliding;
        private bool isDashing;
        private bool isWallJumping;
        
        public bool isDead { get; private set; }

        public void SetDeadState(bool state)
        {
            isDead = state;
            rb.velocity = Vector2.zero;
            rb.isKinematic = state;

            if (state)
            {
                animator.SetTrigger("isDead");
            }
        }

        public void PlayTakeHitAnimation()
        {
            animator.SetTrigger("TakeHit");
        }

        public void PlayTakeHitCriticalAnimation()
        {
            animator.SetTrigger("TakeHitCritical");
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

            inputManager.OnJump += HandleJump;
            inputManager.OnDash += () => StartCoroutine(Dash());
        }

        private void Update()
        {
            if (isDead) return;

            moveInputX = inputManager.MoveInput.x;
            UpdateGroundCheck();
            UpdateWallCheck();
            HandleWallSlide();
            FlipIfNeeded();
            UpdateAnimations();
        }

        private void FixedUpdate()
        {
            if (isDead) return;
            ApplyMovement();
        }

        private void UpdateGroundCheck()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, floorLayer);

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

        private void UpdateWallCheck()
        {
            isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
        }

        private void HandleWallSlide()
        {
            isWallSliding = isTouchingWall && !isGrounded && Mathf.Abs(moveInputX) > 0.1f && !isWallJumping;

            if (isWallSliding)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }

        private void HandleJump()
        {
            if (isDead) return;

            if (isWallSliding)
            {
                StartCoroutine(PerformWallJump());
                return;
            }

            if (coyoteTimeCounter <= 0 && jumpCount <= 0) return;

            PerformJump();
        }

        private void PerformJump()
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            coyoteTimeCounter = 0f;
            jumpCount--;
            animator.SetTrigger("Jump");
        }

        private IEnumerator PerformWallJump()
        {
            isWallJumping = true;

            float direction = isFacingRight ? -1f : 1f;

            // Jump away from wall and upward
            rb.velocity = new Vector2(direction * wallJumpForce, jumpForce);

            // Flip player if needed
            if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
                Flip();

            jumpCount = maxJumps; // Reset jump count after wall jump
            animator.SetTrigger("Jump");

            yield return new WaitForSeconds(wallJumpDuration);
            isWallJumping = false;
        }

        private IEnumerator Dash()
        {
            if (isDashing) yield break;

            isDashing = true;
            rb.gravityScale = 0f;
            float direction = isFacingRight ? 1f : -1f;
            rb.velocity = new Vector2(direction * dashSpeed, 0f);
            animator.SetTrigger("Dash");

            yield return new WaitForSeconds(dashDuration);

            rb.gravityScale = fallMultiplier;
            rb.velocity = new Vector2(0, rb.velocity.y);
            isDashing = false;
        }

        private void ApplyMovement()
        {
            if (isDashing || isWallJumping) return;

            float speed = inputManager.IsSprinting ? sprintSpeed : maxSpeed;
            float targetSpeed = moveInputX * speed;
            float speedDiff = targetSpeed - rb.velocity.x;
            float accelRate = Mathf.Abs(targetSpeed) > 0.1f ? acceleration : deceleration;
            rb.AddForce(Vector2.right * speedDiff * accelRate, ForceMode2D.Force);
        }

        private void FlipIfNeeded()
        {
            if (isWallJumping) return;

            if ((moveInputX > 0 && !isFacingRight) || (moveInputX < 0 && isFacingRight))
                Flip();
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        private void UpdateAnimations()
        {
            if (isDead) return;

            animator.SetBool("isRunning", Mathf.Abs(moveInputX) > 0.1f);
            animator.SetBool("isJumping", !isGrounded && rb.velocity.y > 0f);
            animator.SetBool("isFalling", !isGrounded && rb.velocity.y < 0f);
            animator.SetBool("isWallSliding", isWallSliding);
        }

        public void FaceDirection(bool faceRight)
        {
            if (isFacingRight != faceRight)
                Flip();
        }
    }
}
