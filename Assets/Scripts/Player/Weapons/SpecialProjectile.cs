using UnityEngine;
using Common;

namespace Player.Weapons
{
    public class SpecialProjectile : MonoBehaviour
    {
        private float speed;
        private Vector2 direction;
        private Rigidbody2D rb;

        [SerializeField]
        private float lifeTime = 2f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Launch(Vector2 dir, float spd)
        {
            direction = dir.normalized;
            speed = spd;
            rb.velocity = direction * speed;
            Destroy(gameObject, lifeTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IFreeable freeable))
            {
                freeable.Free(); // This triggers the NPC's freeing logic
                Destroy(gameObject);
            }
        }
    }
}