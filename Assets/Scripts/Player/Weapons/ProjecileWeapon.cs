using UnityEngine;

namespace Player.Weapons
{
    public class ProjectileWeapon : Weapon
    {
        [SerializeField, Tooltip("Projectile prefab to instantiate.")]
        private GameObject projectilePrefab;

        [SerializeField, Tooltip("Spawn point for the projectile.")]
        private Transform firePoint;

        [SerializeField, Tooltip("Speed of the projectile.")]
        private float projectileSpeed = 10f;

        [SerializeField, Tooltip("Damage dealt by the projectile.")]
        private int damage = 10;
        [SerializeField] private AudioClip fireSFX;
        private Animator animator;
        private AudioSource audioSource;
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        protected override void Attack()
        {
            if (!IsReadyToFire()) return;

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            InitializeProjectile(projectile);
            if (animator != null)
            {
                animator.SetTrigger("Fire"); // Only if you have a fire animation layered
            }

            if (audioSource && fireSFX)
            {
                audioSource.PlayOneShot(fireSFX);
            }
        }

        private bool IsReadyToFire()
        {
            if (projectilePrefab == null || firePoint == null)
            {
                Debug.LogWarning("ProjectileWeapon is missing firePoint or prefab.");
                return false;
            }
            return true;
        }

        private void InitializeProjectile(GameObject projectile)
        {
            var projScript = projectile.GetComponent<Projectile>();
            if (projScript == null)
            {
                Debug.LogWarning("Projectile script missing on prefab.");
                return;
            }

            Vector2 direction = GetFacingDirection(transform);
            projScript.Fire(direction, projectileSpeed, damage);

            projectile.transform.right = direction;
            Debug.Log("Projectile fired.");
        }

        private static Vector2 GetFacingDirection(Transform weaponTransform)
        {
            return weaponTransform.lossyScale.x > 0 ? Vector2.right : Vector2.left;
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
