using UnityEngine;
using System.Collections;
using Common;
using UI;
using Game;

namespace Enemy.Bosses.NightBorne
{
    [RequireComponent(typeof(BossHealth), typeof(Rigidbody2D), typeof(Animator))]
    public class NightBorneCore : MonoBehaviour, IDamageable
    {
        // Base references and state variables
        [Header("Core References")]
        [SerializeField] protected Transform player;
        [SerializeField] protected BossWeaponHitbox weaponHitbox;

        protected Rigidbody2D rb;
        protected Animator animator;
        protected BossHealth health;
        protected NightBorneAnimation animationHandler;

        protected bool isDead;
        protected bool isActive;
        protected Vector3 initialScale;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            health = GetComponent<BossHealth>();
            animationHandler = GetComponent<NightBorneAnimation>();

            initialScale = new Vector3(1f, 1f, 1f);
            transform.localScale = initialScale;

            if (player == null)
                player = GameObject.FindWithTag("Player").transform;
        }

        public void ActivateBoss() => isActive = true;

        public virtual void TakeDamage(int amount)
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

        protected static IEnumerator PauseTime()
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(0.08f);
            Time.timeScale = 1f;
        }

        protected virtual void Die()
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

        protected IEnumerator DestroyAfterDeathAnimation()
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
    }
}