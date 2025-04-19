namespace Enemy
{
    public interface IEnemyAnimator
    {
        void PlayIdle();
        void PlayMove();
        void PlayAttack();
        void PlayDeath();
        void PlayJump();
    }
}