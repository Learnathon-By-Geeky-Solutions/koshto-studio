using UnityEngine;

namespace Player.Weapons
{
    public class ProjectileWeapon : Weapon
    {
        [SerializeField]
        [Tooltip("Projectile prefab to instantiate.")]
        private GameObject projectilePrefab;

        [SerializeField]
        [Tooltip("Spawn point for the projectile.")]
        private Transform firePoint;

        [SerializeField]
        [Tooltip("Speed of the projectile.")]
        private float projectileSpeed = 10f;

        [SerializeField]
        [Tooltip("Damage dealt by the projectile.")]
        private int damage = 10;

        protected override void Attack()
        {
            if (projectilePrefab == null || firePoint == null)
            {
                Debug.LogWarning("ProjectileWeapon is missing references.");
                return;
            }

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Projectile projScript = projectile.GetComponent<Projectile>();

            if (projScript != null)
            {
                Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
                projScript.Fire(direction, projectileSpeed, damage);
            }

            Debug.Log("Projectile fired.");
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (firePoint == null) return;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.right * 2f);
        }
#endif
    }
}