using UnityEngine;
using System.Collections;
using Common;
using UI;
using Game;

namespace Enemy.Bosses.NightBorne
{
    [RequireComponent(typeof(BossHealth))]
    public class NightBorneController : MonoBehaviour, IDamageable
    {
        [Header("References")]
        public Transform player;

        [Header("Chase")]
        public float runSpeed = 2f;

        [Header("Attack")]
        public float attackRange = 1.8f;
        public float attackCooldown = 1.2f;
        private bool isAttacking = false;

        [Header("Charge")]
        public float chargeRange = 5f;
        public float chargeSpeed = 10f;
        public float chargeWindUp = 0.5f;
        public float chargeDuration = 1.0f;

        private Animator animator;
        private Rigidbody2D rb;
        private BossHealth health;
        private NightBorneAnimation animationHandler;

        private bool isCharging = false;
        private bool isDead = false;
        private bool isActive = false;

        public void ActivateBoss()
        {
            isActive = true;
        }

        private int healthCheckpoint = 75;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            health = GetComponent<BossHealth>();
            animationHandler = GetComponent<NightBorneAnimation>();
        }

        private void Update()
        {
            if (isDead || isCharging || isAttacking || player == null) return;
            if (!isActive) return;


            float distance = Vector2.Distance(transform.position, player.position);

            if (health.CurrentHealth <= health.MaxHealth * (healthCheckpoint / 100f))
            {
                StartCoroutine(ChargeRoutine());
                healthCheckpoint -= 25;
                return;
            }

            if (distance <= attackRange)
            {
                StartCoroutine(AttackRoutine());
            }
            else
            {
                Chase();
            }
        }

        private void Chase()
        {
            animationHandler.PlayRun();
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * runSpeed, rb.velocity.y);
            Flip(direction.x);
        }

        private IEnumerator AttackRoutine()
        {
            isAttacking = true;
            rb.velocity = Vector2.zero;

            animationHandler.PlayAttack();
            yield return new WaitForSeconds(0.2f); // delay before hit check

            if (Vector2.Distance(transform.position, player.position) <= attackRange)
            {
                if (player.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(25);
                    StartCoroutine(PauseTime());
                }
            }

            yield return new WaitForSeconds(attackCooldown);
            isAttacking = false;
        }

        private IEnumerator ChargeRoutine()
        {
            isCharging = true;
            animationHandler.PlayCharge();

            yield return new WaitForSeconds(chargeWindUp);
            rb.velocity = new Vector2(transform.localScale.x * chargeSpeed, rb.velocity.y);

            yield return new WaitForSeconds(chargeDuration);
            rb.velocity = Vector2.zero;
            isCharging = false;
        }

        public void TakeDamage(int amount)
        {
            if (isDead) return;

            health.TakeDamage(amount);
            animationHandler.PlayHit();

            StartCoroutine(PauseTime());

            if (health.CurrentHealth <= 0)
            {
                Die();
            }
        }
        
        private IEnumerator DestroyAfterDeathAnimation()
        {
            // Wait until death animation has fully played
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // Wait until we’re actually in the Death state
            while (!stateInfo.IsName("Death"))
            {
                yield return null;
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            }

            // Then wait until the animation is finished
            while (stateInfo.normalizedTime < 1f)
            {
                yield return null;
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            }

            Destroy(gameObject);
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            rb.velocity = Vector2.zero;

            animationHandler.PlayDeath();

            Time.timeScale = 1f;

            InputManager input = FindObjectOfType<InputManager>();
            input?.EnablePlayerInput();

            BossMusicManager.Instance?.StopMusic();
            BossHealthBarUI.Instance?.Hide();

            // 👇 Wait for animation length before destroying
            StartCoroutine(DestroyAfterDeathAnimation());
        }

        private IEnumerator PauseTime()
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(0.08f);
            Time.timeScale = 1f;
        }

        private void Flip(float directionX)
        {
            if (directionX > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (directionX < 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (isDead) return;

            if (collision.gameObject.CompareTag("Player") && !isAttacking && !isCharging)
            {
                if (collision.gameObject.TryGetComponent(out IDamageable playerDamageable))
                {
                    playerDamageable.TakeDamage(15);
                    StartCoroutine(PauseTime());
                }

                // Optionally show death if player dies inside player script
                var playerHealth = collision.gameObject.GetComponent<Health>();
                if (playerHealth != null && playerHealth.GetCurrentHealth() <= 0)
                {
                    GameOverUI gameOver = FindObjectOfType<GameOverUI>();
                    gameOver?.Show();
                }
            }
        }
    }
}
