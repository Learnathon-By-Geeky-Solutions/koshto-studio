using UnityEngine;
using System.Collections;
using Common;
using Player.Input;

namespace Player
{
    public class PlayerHitbox : MonoBehaviour, IDamageable
    {
        [SerializeField] private float invulnerabilityDuration = 0.5f;
        private bool isInvulnerable;

        private IEnumerator InvulnerabilityFrame()
        {
            isInvulnerable = true;
            yield return new WaitForSeconds(invulnerabilityDuration);
            isInvulnerable = false;
        }
        private Health playerHealth;
        private PlayerController playerController;

        private float lastHitTime;
        private const float criticalHitThreshold = 0.5f;

        private void Awake()
        {
            playerHealth = GetComponentInParent<Health>();
            playerController = GetComponentInParent<PlayerController>();

            if (playerHealth == null)
                Debug.LogError("PlayerHitbox couldn't find Health component in parent!");

            if (playerController == null)
                Debug.LogError("PlayerHitbox couldn't find PlayerController component in parent!");
        }

        public void TakeDamage(int damage)
        {
            if (playerHealth == null || playerController == null) return;

            playerHealth.TakeDamage(damage);

            float timeSinceLastHit = Time.time - lastHitTime;

            if (timeSinceLastHit <= criticalHitThreshold)
            {
                playerController.PlayTakeHitCriticalAnimation();
            }
            else
            {
                playerController.PlayTakeHitAnimation();
            }

            lastHitTime = Time.time;
        }
    }
}
