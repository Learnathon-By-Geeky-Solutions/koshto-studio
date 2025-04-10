using UnityEngine;
using Enemy;

namespace Player.Weapons
{
    public class Projectile : MonoBehaviour
    {
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
            Vector2 direction = dir.normalized;
            float speed = spd; // <-- Now a local variable
            damage = dmg;
            rb.velocity = direction * speed;
            Destroy(gameObject, lifeTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Common.IDamageable damageable))
            {
                damageable.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
