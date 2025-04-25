using UnityEngine;

namespace Player.Weapons
{
    public class SpecialWeapon : Weapon
    {
        [SerializeField, Tooltip("Projectile prefab to instantiate.")]
        private GameObject projectilePrefab;

        [SerializeField, Tooltip("Spawn point for the projectile.")]
        private Transform firePoint;

        [SerializeField, Tooltip("Speed of the projectile.")]
        private float projectileSpeed = 8f;

        [SerializeField, Tooltip("Sound played when firing.")]
        private AudioClip fireSFX;

        private Animator animator;
        private AudioSource audioSource;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        protected override void Attack()
        {
            if (!IsReadyToFire()) return;

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            InitializeProjectile(projectile);

            animator?.SetTrigger("Fire");

            if (audioSource && fireSFX)
            {
                audioSource.PlayOneShot(fireSFX);
            }
        }

        private bool IsReadyToFire()
        {
            if (projectilePrefab == null || firePoint == null)
            {
                Debug.LogWarning("SpecialWeapon is missing firePoint or prefab.");
                return false;
            }
            return true;
        }

        private void InitializeProjectile(GameObject projectile)
        {
            var projScript = projectile.GetComponent<SpecialProjectile>();
            if (projScript == null)
            {
                Debug.LogWarning("SpecialProjectile script missing on prefab.");
                return;
            }

            Vector2 direction = GetFacingDirection(transform);
            projScript.Launch(direction, projectileSpeed);

            projectile.transform.right = direction;

            Debug.Log("Special projectile fired.");
        }

        private static Vector2 GetFacingDirection(Transform weaponTransform)
        {
            return weaponTransform.lossyScale.x > 0 ? Vector2.right : Vector2.left;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (firePoint == null) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.right * 2f);
        }
#endif
    }
}
