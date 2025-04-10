using System.Collections;
using UnityEngine;

namespace Player.input
{
    public partial class Player
    {
        [Header("Wall Jump & Slide")]
        [SerializeField] private float wallSlideSpeed = 2f;
        [SerializeField] private float wallJumpForce = 10f;
        [SerializeField] private Transform wallCheck;
        [SerializeField] private WeaponHandler weaponHandler;

        [Header("Ground Detection")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;

        public float WallSlideSpeed => wallSlideSpeed;
        public float WallJumpForce => wallJumpForce;
        public Transform WallCheck => wallCheck;
        public Transform GroundCheck => groundCheck;
        public LayerMask GroundLayer => groundLayer;

        private void HandleWallSlide()
        {
            isWallSliding = ShouldSlide();

            if (!isWallSliding) return;

            ApplyWallSlide();
        }

        private bool ShouldSlide()
        {
            if (isGrounded) return false;
            if (!IsTouchingWall()) return false;
            if (Mathf.Abs(moveInputX) <= 0.1f) return false;

            return true;
        }

        private bool IsTouchingWall()
        {
            return Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);
        }

        private void ApplyWallSlide()
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }

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
    }
}