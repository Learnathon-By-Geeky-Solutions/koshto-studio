using UnityEngine;
using Common;

namespace Enemy.Bosses.NightBorne
{
    public class BossHealth : MonoBehaviour
    {
        [field: SerializeField] public int MaxHealth { get; private set; } = 300;
        public int CurrentHealth { get; private set; }

        public System.Action OnHealthChanged;

        private void Awake()
        {
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(int amount)
        {
            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
            OnHealthChanged?.Invoke();
        }
    }
}