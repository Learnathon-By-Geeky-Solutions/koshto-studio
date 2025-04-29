using UnityEngine;
using System.Collections;

namespace Enemy.Bosses.NightBorne
{
    public class NightBorneController : NightBornePhaseManager
    {
        [Header("Charge")]
        [SerializeField] private float chargeRange = 5f;
        [SerializeField] private float chargeSpeed = 10f;
        [SerializeField] private float chargeWindUp = 0.5f;
        [SerializeField] private float chargeDuration = 1.0f;

        private bool isCharging = false;

        private void Update()
        {
            if (!CanAct()) return;

            float distance = Vector2.Distance(transform.position, player.position);
            UpdateFacingDirection(player.position.x - transform.position.x);

            CheckPhaseTransition();

            if (ShouldCharge(distance))
            {
                StartCoroutine(ChargeRoutine());
                healthCheckpoint -= 25;
                return;
            }

            if (distance <= attackRange)
            {
                StartCoroutine(AttackRoutine());
            }
            else if (!isBackingOff)
            {
                if (distance > stoppingDistance)
                {
                    ChasePlayer();
                }
                else
                {
                    Idle();
                }
            }

            HandleReapproachCooldown();
        }

        private bool CanAct()
        {
            return !isDead && !isCharging && !isAttacking && isActive && player != null;
        }

        private IEnumerator ChargeRoutine()
        {
            isCharging = true;
            animationHandler.PlayCharge();

            yield return new WaitForSeconds(chargeWindUp);

            rb.velocity = new Vector2(facingDirection * chargeSpeed, rb.velocity.y);

            yield return new WaitForSeconds(chargeDuration);

            rb.velocity = Vector2.zero;
            isCharging = false;
        }
    }
}