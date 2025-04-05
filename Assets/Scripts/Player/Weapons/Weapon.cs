using UnityEngine;

namespace Player.Weapons
{
    /// <summary>
    /// Abstract base class for all weapons.
    /// </summary>
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Cooldown time between attacks in seconds.")]
        private float attackCooldown = 0.5f;

        protected bool CanAttack = true;

        /// <summary>
        /// Called to trigger an attack.
        /// </summary>
        public void TryAttack()
        {
            if (!CanAttack)
            {
                return;
            }

            Attack();
            StartCoroutine(AttackCooldown());
        }

        /// <summary>
        /// Perform the attack logic (implemented in derived classes).
        /// </summary>
        protected abstract void Attack();

        private System.Collections.IEnumerator AttackCooldown()
        {
            CanAttack = false;
            yield return new WaitForSeconds(attackCooldown);
            CanAttack = true;
        }
    }
}