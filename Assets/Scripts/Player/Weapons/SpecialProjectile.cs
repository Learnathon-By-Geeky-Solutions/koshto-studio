using UnityEngine;
using Common;

namespace Player.Weapons
{
    public class SpecialProjectile : MonoBehaviour
    {
        private float speed;
        private Vector2 direction;

        [SerializeField] private float lifetime = 3f;

        public void Launch(Vector2 dir, float spd)
        {
            direction = dir.normalized;
            speed = spd;
            Destroy(gameObject, lifetime); // Destroy after lifetime
        }

        private void Update()
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var freeable = other.GetComponent<IFreeable>();
            if (freeable != null)
            {
                freeable.Free(); // Trigger the freeing logic
                Destroy(gameObject);
            }

            // Optional: Add feedback (particles, SFX) here
        }
    }
}