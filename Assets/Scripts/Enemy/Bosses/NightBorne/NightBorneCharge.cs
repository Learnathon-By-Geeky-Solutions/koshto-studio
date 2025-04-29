using System.Collections;
using UnityEngine;

namespace Enemy.Bosses.NightBorne
{
    public class NightBorneCharge : NightBornePhaseManager
    {
        [Header("Charge")]
        [SerializeField] private float chargeRange = 5f;
        [SerializeField] private float chargeSpeed = 10f;
        [SerializeField] private float chargeWindUp = 0.5f;
        [SerializeField] private float chargeDuration = 1f;

        private bool isCharging;
        private int healthCheckpoint = 75;

        public IEnumerator ChargeRoutine()
        {
            isCharging = true;
            yield return new WaitForSeconds(chargeWindUp);

            rb.velocity = new Vector2(facingDirection * chargeSpeed, rb.velocity.y);
            yield return new WaitForSeconds(chargeDuration);

            rb.velocity = Vector2.zero;
            isCharging = false;
            healthCheckpoint -= 25;
        }

        public bool ShouldCharge()
        {
            bool healthLowEnough = health.CurrentHealth <= health.MaxHealth * (healthCheckpoint / 100f);
            bool playerInFront = Mathf.Sign(player.position.x - transform.position.x) == facingDirection;
            return healthLowEnough && playerInFront &&
                   Vector2.Distance(transform.position, player.position) <= chargeRange;
        }

        public bool IsCharging => isCharging;
    }
}