using UnityEngine;
using Common;

namespace Enemy
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Health))]
    public class DummyEnemy : MonoBehaviour
    {
        private void Awake()
        {
            var rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.bodyType = RigidbodyType2D.Static;

            var col = GetComponent<BoxCollider2D>();
            col.isTrigger = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log($"{gameObject.name} collided with {collision.gameObject.name}");
        }
    }
}