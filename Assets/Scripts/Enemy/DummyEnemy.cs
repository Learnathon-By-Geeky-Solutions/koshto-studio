using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(EnemyHealth))]
    public class DummyEnemy : MonoBehaviour
    {
        private void Awake()
        {
            // Make sure Rigidbody2D is static and not affected by gravity
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.bodyType = RigidbodyType2D.Static;

            // Ensure proper 2D collision
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.isTrigger = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log($"{gameObject.name} collided with {collision.gameObject.name}");
        }
    }
}