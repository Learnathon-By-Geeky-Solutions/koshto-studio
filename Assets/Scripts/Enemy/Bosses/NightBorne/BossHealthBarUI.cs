using UnityEngine;
using UnityEngine.UI;

namespace Enemy.Bosses.NightBorne
{
    public class BossHealthBarUI : MonoBehaviour
    {
        public static BossHealthBarUI Instance;

        [SerializeField] private Slider healthSlider;
        [SerializeField] private GameObject barContainer;

        private void Awake()
        {
            Instance = this;
        }

        public void Init(int maxHealth)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
            barContainer.SetActive(true);
        }

        public void UpdateHealth(int currentHealth)
        {
            healthSlider.value = currentHealth;
        }

        public void Hide()
        {
            barContainer.SetActive(false);
        }
    }
}