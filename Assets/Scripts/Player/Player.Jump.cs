using UnityEngine;

namespace Player.input
{
    public partial class Player
    {
        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float coyoteTime = 0.1f;
        [SerializeField] private float lowJumpMultiplier = 2f;
        [SerializeField] private float fallMultiplier = 2.5f;
        [SerializeField] private int maxJumps = 2;

        private float coyoteTimeCounter;
        private bool isGrounded;
        private int jumpCount;

        public float JumpForce => jumpForce;
        public float CoyoteTime => coyoteTime;
        public float LowJumpMultiplier => lowJumpMultiplier;
        public float FallMultiplier => fallMultiplier;
        public int MaxJumps => maxJumps;
        public float CoyoteTimeCounter { get => coyoteTimeCounter; private set => coyoteTimeCounter = value; }
        public bool IsGrounded { get => isGrounded; private set => isGrounded = value; }
        public int JumpCount { get => jumpCount; private set => jumpCount = value; }

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
    }
}