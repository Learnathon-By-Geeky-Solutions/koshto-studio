using UnityEngine;
using Common;
using Player.Input; // Import PlayerController

namespace Player.Weapons
{
    public class MeleeWeapon : Weapon
    {
        [SerializeField] private int damage = 25;
        [SerializeField] private Vector2 attackRange = new Vector2(1f, 1f);
        [SerializeField] private Transform attackOrigin;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private AudioClip attackSFX;

        private AudioSource audioSource;
        private PlayerController playerController;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            playerController = GetComponentInParent<PlayerController>();

            if (playerController == null)
            {
                Debug.LogError("MeleeWeapon: No PlayerController found in parent!");
            }
        }

        protected override void Attack()
        {
            if (!IsConfigured())
            {
                Debug.LogWarning("MeleeWeapon: Not properly configured.");
                return;
            }
            PerformAttack();
        }

        private void PerformAttack()
        {
            if (playerController != null)
            {
                Debug.LogWarning("Attack origin not set.");
            }

            if (audioSource && attackSFX)
            {
                audioSource.PlayOneShot(attackSFX);
            }

            var hits = Physics2D.OverlapBoxAll(attackOrigin.position, attackRange, 0f, enemyLayer);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damage);
                }
            }
        }

        private bool IsConfigured()
        {
            return attackOrigin != null && attackRange.x > 0 && attackRange.y > 0;
        }
    }
}