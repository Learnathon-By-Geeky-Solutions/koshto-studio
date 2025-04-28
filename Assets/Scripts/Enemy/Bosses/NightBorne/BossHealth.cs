using UnityEngine;
using Common;

namespace Enemy.Bosses.NightBorne
{
    public class BossHealth : MonoBehaviour
    {
        [field: SerializeField] public int MaxHealth { get; private set; } = 300;
        public int CurrentHealth { get; private set; }

        [SerializeField] private System.Action onHealthChanged;

        public System.Action OnHealthChanged
        {
            get => onHealthChanged;
            set => onHealthChanged = value;
        }

        private void Awake()
        {
            CurrentHealth = MaxHealth;

            // Initialize health bar
            if (BossHealthBarUI.Instance != null)
            {
                BossHealthBarUI.Instance.Init(MaxHealth);
            }
        }

        public void TakeDamage(int amount)
        {
            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
            OnHealthChanged?.Invoke();

            // Update health bar when health changes
            if (BossHealthBarUI.Instance != null)
            {
                BossHealthBarUI.Instance.UpdateHealth(CurrentHealth, MaxHealth);
            }
        }
    }
}
