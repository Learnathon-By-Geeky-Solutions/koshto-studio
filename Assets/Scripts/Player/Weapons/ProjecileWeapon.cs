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

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Projectile projScript = projectile.GetComponent<Projectile>();

            if (projScript != null)
            {
                bool isFacingRight = transform.lossyScale.x > 0;
                Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

                projScript.Fire(direction, projectileSpeed, damage);

                // Optional: rotate the projectile sprite to match the direction it's flying
                projectile.transform.right = direction;
            }
            Debug.Log("Projectile fired.");
        }
        // public override void FlipFirePoint(bool facingRight)
        // {
        //     if (firePoint != null)
        //     {
        //         Vector3 pos = firePoint.localPosition;
        //         pos.x = Mathf.Abs(pos.x) * (facingRight ? 1 : -1);
        //         firePoint.localPosition = pos;
        //     }
        // }

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