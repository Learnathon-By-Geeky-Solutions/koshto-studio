using UnityEngine;
using Common;
using System.Diagnostics.CodeAnalysis;


namespace Player.Weapons
{
    public class SpecialProjectile : MonoBehaviour
    {
        private float speed;
        private Rigidbody2D rb;

        [SerializeField]
        private float lifeTime = 2f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Launch(Vector2 dir, float spd)
        {
            Vector2 direction = dir.normalized;
            speed = spd;
            rb.velocity = direction * speed;
            Destroy(gameObject, lifeTime);
        }

        [SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed", Justification = "Unity event method must not be static")]
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IFreeable freeable))
            {
                freeable.Free();
                Destroy(gameObject);
            }
        }
    }
}
