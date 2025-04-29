using UnityEngine;

namespace Enemy.Bosses.NightBorne
{
    public class NightBornePhaseManager : NightBorneCombat
    {
        [Header("Phase Transition")]
        [SerializeField] protected int healthCheckpoint = 75;

        protected void CheckPhaseTransition()
        {
            if (!inSecondPhase && health.CurrentHealth <= health.MaxHealth * 0.5f)
            {
                EnterSecondPhase();
            }
        }

        protected void EnterSecondPhase()
        {
            inSecondPhase = true;
            transform.localScale = initialScale * 2f;
            weaponHitbox.IncreaseBaseDamageByPercentage(25);
            Debug.Log("NightBorne has entered second phase!");
        }

        protected bool ShouldCharge(float distance, float chargeRange = 5f)
        {
            bool healthLowEnough = health.CurrentHealth <= health.MaxHealth * (healthCheckpoint / 100f);
            bool playerInFront = Mathf.Sign(player.position.x - transform.position.x) == facingDirection;
            return healthLowEnough && playerInFront && distance <= chargeRange;
        }
    }
}