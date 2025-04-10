using System.Collections;
using UnityEngine;

namespace Player.input
{
    public partial class Player
    {
        [Header("Dash Settings")]
        [SerializeField] private float dashSpeed = 20f;
        [SerializeField] private float dashDuration = 0.2f;

        public float DashSpeed => dashSpeed;
        public float DashDuration => dashDuration;

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
    }
}