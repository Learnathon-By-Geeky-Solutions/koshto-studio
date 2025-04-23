using UnityEngine;

namespace Enemy
{
    public class ZombieEnemy : EnemyBase
    {
        protected override void OnDeath()
        {
            base.OnDeath();
            // TODO: Add custom death particles or sounds for zombie
        }
    }
}