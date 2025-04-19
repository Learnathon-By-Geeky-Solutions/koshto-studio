// Scripts/Enemy/Types/ZombieEnemy.cs
using UnityEngine;

namespace Enemy
{
    public class ZombieEnemy : EnemyBase
    {
        // You can override specific behavior, like slowing movement while attacking
        protected override void OnDeath()
        {
            base.OnDeath();
            // Add custom death particles or sounds
        }
    }
}