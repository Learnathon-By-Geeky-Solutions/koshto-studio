using UnityEngine;
using Enemy;

namespace Player.Weapons
{
    public class Projectile : MonoBehaviour
    {
        private float speed;
        private int damage;
        private Rigidbody2D rb;

        [SerializeField]
        private float lifeTime = 1f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Fire(Vector2 dir, float spd, int dmg)
        {
            Vector2 direction = dir.normalized; // <-- Local variable now
            speed = spd;
            damage = dmg;
            rb.velocity = direction * speed;
            Destroy(gameObject, lifeTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out EnemyHealth enemyHealth))
            {
                enemyHealth.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
