using UnityEngine;
using Common;

namespace Player.Weapons
{
    public class SpecialWeapon : Weapon
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float projectileSpeed = 8f;
        [SerializeField] private AudioClip fireSFX;

        private Animator animator;
        private AudioSource audioSource;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        protected override void Attack()
        {
            if (!CanFire()) return;
            FireProjectile();
            // No cooldown reset or ammo logic unless needed
        }

        private void FireProjectile()
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Vector2 direction = GetFacingDirection(transform);
            projectile.transform.right = direction;

            var specialProjectile = projectile.GetComponent<SpecialProjectile>();
            if (specialProjectile != null)
            {
                specialProjectile.Launch(direction, projectileSpeed);
            }

            if (animator != null)
            {
                animator.SetTrigger("Fire"); // Only if you have a fire animation layered
            }

            if (audioSource && fireSFX)
            {
                audioSource.PlayOneShot(fireSFX);
            }
        }

        private bool CanFire()
        {
            return projectilePrefab != null && firePoint != null;
        }

        private static Vector2 GetFacingDirection(Transform weaponTransform)
        {
            return weaponTransform.lossyScale.x > 0 ? Vector2.right : Vector2.left;
        }
    }
}
