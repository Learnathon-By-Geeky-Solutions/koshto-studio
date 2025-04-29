using UnityEngine;
using System.Collections;

namespace Enemy.Bosses.NightBorne
{
    public class NightBorneMovement : NightBorneCore
    {
        [Header("Chase")]
        [SerializeField] protected float runSpeed = 2f;
        [SerializeField] protected float stoppingDistance = 1.5f;

        protected int facingDirection = 1;
        protected bool isBackingOff = false;
        protected float reapproachTimer = 0f;

        protected void ChasePlayer()
        {
            animationHandler.PlayRun();
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * runSpeed, rb.velocity.y);

            UpdateFacingDirection(direction.x);
        }

        protected void Idle()
        {
            animationHandler.PlayIdle();
            rb.velocity = Vector2.zero;
        }

        protected void UpdateFacingDirection(float moveDirectionX)
        {
            int newFacingDirection = facingDirection;

            if (moveDirectionX > 0)
            {
                newFacingDirection = 1;
            }
            else if (moveDirectionX < 0)
            {
                newFacingDirection = -1;
            }

            if (newFacingDirection != facingDirection)
            {
                facingDirection = newFacingDirection;
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * facingDirection, transform.localScale.y, 1);
            }
        }

        protected void HandleReapproachCooldown()
        {
            if (isBackingOff)
            {
                reapproachTimer -= Time.deltaTime;
                if (reapproachTimer <= 0f)
                {
                    isBackingOff = false;
                }
            }
        }

        protected IEnumerator SmoothBackOff(Vector2 targetPosition, float reapproachDelay = 0.8f)
        {
            isBackingOff = true;
            reapproachTimer = reapproachDelay;

            float elapsed = 0f;
            float duration = 0.4f;
            Vector2 start = transform.position;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector2.Lerp(start, targetPosition, elapsed / duration);
                yield return null;
            }

            transform.position = targetPosition;
        }
    }
}