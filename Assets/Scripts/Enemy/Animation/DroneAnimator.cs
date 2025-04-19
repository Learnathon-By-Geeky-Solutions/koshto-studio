using UnityEngine;

namespace Enemy
{
    public class DroneAnimator : MonoBehaviour, IEnemyAnimator
    {
        private Animator animator;

        private void Awake() => animator = GetComponent<Animator>();

        public void PlayIdle() => animator.SetBool("isMoving", false);
        public void PlayMove() => animator.SetBool("isMoving", true);
        public void PlayAttack() => animator.SetTrigger("Attack");
        public void PlayDeath() => animator.SetTrigger("isDead");
        public void PlayJump() { } // not applicable
    }
}