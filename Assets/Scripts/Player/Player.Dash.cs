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

            StartDash();

            yield return new WaitForSeconds(dashDuration);

            EndDash();
        }

        private void StartDash()
        {
            isDashing = true;
            rb.gravityScale = 0;
            float direction = isFacingRight ? 1f : -1f;
            rb.velocity = new Vector2(direction * dashSpeed, 0f);
            animator.SetTrigger("Dash");
        }

        private void EndDash()
        {
            rb.gravityScale = fallMultiplier;
            rb.velocity = new Vector2(0, rb.velocity.y);
            isDashing = false;
        }
    }
}