using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.input 
{

    public class Player : MonoBehaviour
    {
        private Rigidbody2D rb;
        private Animator animator;
        private PlayerControls controls;

        // Input Variables
        private float moveInputX;
        private bool isJumping, isDashing, isSprinting, isWallSliding, isFacingRight = true;

        [Header("Movement Settings")]
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float acceleration = 8f;
        [SerializeField] private float deceleration = 6f;

        public float MaxSpeed
        {
            get => maxSpeed;
            private set => maxSpeed = value;
        }

        public float Acceleration
        {
            get => acceleration;
            private set => acceleration = value;
        }

        public float Deceleration
        {
            get => deceleration;
            private set => deceleration = value;
        }


        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float coyoteTime = 0.1f;
        [SerializeField] private float lowJumpMultiplier = 2f;
        [SerializeField] private float fallMultiplier = 2.5f;
        private float coyoteTimeCounter;
        private bool isGrounded;
        private int jumpCount;
        [SerializeField] private int maxJumps = 2;

        public float JumpForce
        {
            get => jumpForce;
            private set => jumpForce = value;
        }

        public float CoyoteTime
        {
            get => coyoteTime;
            private set => coyoteTime = value;
        }

        public float LowJumpMultiplier
        {
            get => lowJumpMultiplier;
            private set => lowJumpMultiplier = value;
        }

        public float FallMultiplier
        {
            get => fallMultiplier;
            private set => fallMultiplier = value;
        }

        public int MaxJumps
        {
            get => maxJumps;
            private set => maxJumps = value;
        }

        public float CoyoteTimeCounter
        {
            get => coyoteTimeCounter;
            private set => coyoteTimeCounter = value;
        }

        public bool IsGrounded
        {
            get => isGrounded;
            private set => isGrounded = value;
        }

        public int JumpCount
        {
            get => jumpCount;
            private set => jumpCount = value;
        }


        [Header("Wall Jump & Slide")]
        [SerializeField] private float wallSlideSpeed = 2f;
        [SerializeField] private float wallJumpForce = 10f;
        [SerializeField] private Transform wallCheck;
        [SerializeField] private WeaponHandler weaponHandler;

        public float WallSlideSpeed
        {
            get => wallSlideSpeed;
            private set => wallSlideSpeed = value;
        }

        public float WallJumpForce
        {
            get => wallJumpForce;
            private set => wallJumpForce = value;
        }

        public Transform WallCheck
        {
            get => wallCheck;
            private set => wallCheck = value;
        }

        private void HandleWallSlide1()
        {
            // Declare isTouchingWall as a local variable
            bool isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);

            // Rename the local variable to avoid conflict with the class-level variable
            bool isCurrentlyWallSliding = isTouchingWall && !isGrounded && Mathf.Abs(moveInputX) > 0.1f;

            if (isCurrentlyWallSliding)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }

        private void WallJump1()
        {
            // Declare isTouchingWall as a local variable
            bool isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);

            if (isTouchingWall)
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
        }


        [Header("Dash Settings")]
        [SerializeField] private float dashSpeed = 20f;
        [SerializeField] private float dashDuration = 0.2f;

        public float DashSpeed
        {
            get => dashSpeed;
            private set => dashSpeed = value; // You can make the setter private if you don't want it to be set outside
        }

        public float DashDuration
        {
            get => dashDuration;
            private set => dashDuration = value; // Same as above, optional to keep setter private
        }


        [Header("Sprint Settings")]
        [SerializeField] private float sprintSpeed = 8f;

        public float movementSpeed
        {
            get { return sprintSpeed; }
            private set { sprintSpeed = value; } // You can make the setter private if you don't want it to be set outside
        }


        [Header("Ground Detection")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;

        public Transform GroundCheck => groundCheck;
        public LayerMask GroundLayer => groundLayer;



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
                animator.ResetTrigger("Jump");
                animator.Play("Jump", 0, 0f);
                animator.SetTrigger("Jump");
            }
        }

        // ✅ Wall Sliding Logic
        private void HandleWallSlide()
        {
            bool isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);
            float threshold = 0.1f;

            // Assign to the actual class variable
            isWallSliding = isTouchingWall && !isGrounded && Mathf.Abs(moveInputX) > threshold;

            if (isWallSliding)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
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
            if (weaponHandler != null)
                weaponHandler.FlipWeapon(isFacingRight);
        }

        // ✅ Update Animator Parameters
        private void UpdateAnimations()
        {
            animator.SetBool("isRunning", Mathf.Abs(moveInputX) > 0.1f);
            animator.SetBool("isJumping", !isGrounded);
            animator.SetBool("isFalling", !isGrounded && rb.velocity.y < 0);
            animator.SetBool("isWallSliding", isWallSliding);
            if (isGrounded)
            {
                animator.ResetTrigger("Jump");
            }
        }

        private void OnEnable() => controls.Gameplay.Enable();
        private void OnDisable() => controls.Gameplay.Disable();
    }
}