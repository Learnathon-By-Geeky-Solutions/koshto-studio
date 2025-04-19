using UnityEngine;

namespace Enemy
{
    public class ZombieJumpHandler : MonoBehaviour
    {
        [SerializeField] private float jumpForce = 7f;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private Transform frontCheck;

        private Rigidbody2D rb;
        private IEnemyAnimator animator;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<IEnemyAnimator>();
        }

        private void Update()
        {
            // Simple obstacle detection
            Collider2D hit = Physics2D.OverlapCircle(frontCheck.position, 0.2f, obstacleLayer);
            if (hit)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator?.PlayJump();
            }
        }
    }
}