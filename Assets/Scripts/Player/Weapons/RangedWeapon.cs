using UnityEngine;

namespace Player.Weapons
{
    public class RangedWeapon : Weapon
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float fireForce = 10f;
        [SerializeField] private float fireRate = 0.5f;

        private float lastFiredTime;

        protected override void Attack()
        {
            if (Time.time >= lastFiredTime + fireRate)
            {
                Debug.Log("Gun Fired!");
                Fire();
                lastFiredTime = Time.time;
            }
        }

        private void Fire()
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(firePoint.right * fireForce, ForceMode2D.Impulse);
            }

            Debug.Log("Ranged weapon fired.");
        }
    }
}