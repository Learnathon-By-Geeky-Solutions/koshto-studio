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
        [SerializeField] private Transform player;
        public Transform Player => player;

        [Header("Chase")]
        [SerializeField] private float runSpeed = 2f;

        [Header("Attack")]
        [SerializeField] private float attackRange = 1.8f;
        [SerializeField] private float attackCooldown = 1.2f;
        [SerializeField] private int baseDamage = 10; // weapon base damage

        [Header("Charge")]
        [SerializeField] private float chargeRange = 5f;
        [SerializeField] private float chargeSpeed = 10f;
        [SerializeField] private float chargeWindUp = 0.5f;
        [SerializeField] private float chargeDuration = 1.0f;

        [Header("Knockback")]
        [SerializeField] private Vector2 playerKnockbackForce = new Vector2(5f, 2f);
        [SerializeField] private float bossBackOffDistance = 2f;

        [Header("Weapon")]
        [SerializeField] private BossWeaponHitbox weaponHitbox;

        [Header("Chase Settings")]
        [SerializeField] private float stoppingDistance = 1.5f; // minimum distance to player before stopping
        [SerializeField] private float reapproachDelay = 0.8f; // wait after back-off before chasing again

        private bool isBackingOff = false;
        private float reapproachTimer = 0f;

        private Animator animator;
        private Rigidbody2D rb;
        private BossHealth health;
        private NightBorneAnimation animationHandler;

        private bool isCharging = false;
        private bool isDead = false;
        private bool isActive = false;
        private bool isAttacking = false;
        private bool inSecondPhase = false;

        private int healthCheckpoint = 75;
        private int facingDirection = 1; // 1 = right, -1 = left

        private Vector3 initialScale;

        public void ActivateBoss()
        {
            isActive = true;
        }

        public void EnableHitbox()
        {
            if (weaponHitbox != null)
            {
                int damage = GetCurrentDamage();
                weaponHitbox.SetDamage(damage);
                weaponHitbox.EnableHitbox();
            }
        }

        public void DisableHitbox()
        {
            if (weaponHitbox != null)
                weaponHitbox.DisableHitbox();
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            health = GetComponent<BossHealth>();
            animationHandler = GetComponent<NightBorneAnimation>();

            initialScale = new Vector3(1f, 1f, 1f);
            transform.localScale = initialScale;
        }

        private void Update()
        {
            if (!CanAct()) return;

            float distance = Vector2.Distance(transform.position, player.position);

            // Always update facing direction based on player
            float directionX = player.position.x - transform.position.x;
            UpdateFacingDirection(directionX);

            CheckPhaseTransition();

            if (ShouldCharge(distance))
            {
                StartCoroutine(ChargeRoutine());
                healthCheckpoint -= 25;
                return;
            }

            if (distance <= attackRange)
            {
                StartCoroutine(AttackRoutine());
            }
            else if (!isBackingOff)
            {
                if (distance > stoppingDistance)
                {
                    ChasePlayer();
                }
                else
                {
                    Idle();
                }
            }

            HandleReapproachCooldown();
        }
        private void HandleReapproachCooldown()
        {
            if (isBackingOff)
            {
                reapproachTimer -= Time.deltaTime;
                if (reapproachTimer <= 0f)
                {
                    isBackingOff = false;
                }
            }
        }


        private bool CanAct()
        {
            return !isDead && !isCharging && !isAttacking && isActive && player != null;
        }

        private void CheckPhaseTransition()
        {
            if (!inSecondPhase && health.CurrentHealth <= health.MaxHealth * 0.5f)
            {
                EnterSecondPhase();
            }
        }

        private void EnterSecondPhase()
        {
            inSecondPhase = true;

            transform.localScale = initialScale * 2f; // Boss doubles in size
            weaponHitbox.IncreaseBaseDamageByPercentage(25); // +25% damage

            Debug.Log("NightBorne has entered second phase!");
        }

        private bool ShouldCharge(float distance)
        {
            bool healthLowEnough = health.CurrentHealth <= health.MaxHealth * (healthCheckpoint / 100f);
            bool playerInFront = Mathf.Sign(player.position.x - transform.position.x) == facingDirection;
            return healthLowEnough && playerInFront && distance <= chargeRange;
        }

        private void ChasePlayer()
        {
            animationHandler.PlayRun();
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * runSpeed, rb.velocity.y);

            UpdateFacingDirection(direction.x);
        }

        private void UpdateFacingDirection(float moveDirectionX)
        {
            int newFacingDirection = facingDirection;

            if (moveDirectionX > 0)
            {
                newFacingDirection = 1;
            }
            else if (moveDirectionX < 0)
            {
                newFacingDirection = -1;
            }

            if (newFacingDirection != facingDirection)
            {
                facingDirection = newFacingDirection;
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * facingDirection, transform.localScale.y, 1);
            }
        }

        private IEnumerator AttackRoutine()
        {
            isAttacking = true;
            rb.velocity = Vector2.zero;

            animationHandler.PlayAttack();
            yield return new WaitForSeconds(0.2f);

            if (inSecondPhase)
            {
                KnockbackPlayer();
                BackOffAfterAttack();
            }

            yield return new WaitForSeconds(attackCooldown);
            isAttacking = false;
        }

        private IEnumerator ChargeRoutine()
        {
            isCharging = true;
            animationHandler.PlayCharge();

            yield return new WaitForSeconds(chargeWindUp);

            rb.velocity = new Vector2(facingDirection * chargeSpeed, rb.velocity.y);

            yield return new WaitForSeconds(chargeDuration);

            rb.velocity = Vector2.zero;
            isCharging = false;
        }

        private void KnockbackPlayer()
        {
            if (player.TryGetComponent<Rigidbody2D>(out var playerRb))
            {
                Vector2 force = new Vector2(facingDirection * playerKnockbackForce.x, playerKnockbackForce.y);
                playerRb.AddForce(force, ForceMode2D.Impulse);
            }
        }

        private void BackOffAfterAttack()
        {
            if (!inSecondPhase) return;

            isBackingOff = true;
            reapproachTimer = reapproachDelay;

            Vector2 backOffTarget = (Vector2)transform.position + new Vector2(-facingDirection * bossBackOffDistance, 0f);
            StartCoroutine(SmoothBackOff(backOffTarget));
        }

        private IEnumerator SmoothBackOff(Vector2 targetPosition)
        {
            float elapsed = 0f;
            float duration = 0.4f; // seconds to move back
            Vector2 start = transform.position;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector2.Lerp(start, targetPosition, elapsed / duration);
                yield return null;
            }

            transform.position = targetPosition;
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

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            rb.velocity = Vector2.zero;

            animationHandler.PlayDeath();
            Time.timeScale = 1f;

            FindObjectOfType<InputManager>()?.EnablePlayerInput();
            BossMusicManager.Instance?.StopMusic();
            BossHealthBarUI.Instance?.Hide();

            StartCoroutine(DestroyAfterDeathAnimation());

            var victoryHandler = FindObjectOfType<Level.BossVictoryHandler>();
            victoryHandler?.OnBossDefeated();
        }

        private IEnumerator DestroyAfterDeathAnimation()
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            while (!stateInfo.IsName("Death"))
            {
                yield return null;
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            }

            while (stateInfo.normalizedTime < 1f)
            {
                yield return null;
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            }

            Destroy(gameObject);
        }

        private static IEnumerator PauseTime()
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(0.08f);
            Time.timeScale = 1f;
        }

        private int GetCurrentDamage()
        {
            return inSecondPhase ? Mathf.CeilToInt(baseDamage * 1.25f) : baseDamage;
        }

        private void Idle()
        {
            animationHandler.PlayIdle();
            rb.velocity = Vector2.zero;
        }
    }
}
