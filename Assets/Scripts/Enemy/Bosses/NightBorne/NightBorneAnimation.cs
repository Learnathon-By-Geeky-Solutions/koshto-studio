using UnityEngine;

namespace Enemy.Bosses.NightBorne
{
    public class NightBorneAnimation : MonoBehaviour
    {
        private Animator animator;

        private void Awake() => animator = GetComponent<Animator>();

        public void PlayRun() => animator.SetBool("isRunning", true);
        public void PlayAttack() => animator.SetTrigger("Attack");
        public void PlayCharge() => animator.SetTrigger("Charge");
        public void PlayHit() => animator.SetTrigger("Hit");
        public void PlayDeath()
        {
            animator.ResetTrigger("Hit");
            animator.ResetTrigger("Attack");
            animator.ResetTrigger("Charge");

            animator.SetBool("isDead", true); // ✅ This is crucial if your Animator uses a bool
        }
    }
}