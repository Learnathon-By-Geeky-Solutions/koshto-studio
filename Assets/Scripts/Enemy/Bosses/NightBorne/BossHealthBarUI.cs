using UnityEngine;
using UnityEngine.UI;

namespace Enemy.Bosses.NightBorne
{
    public class BossHealthBarUI : MonoBehaviour
    {
        
        private static BossHealthBarUI _instance;
        public static BossHealthBarUI Instance
        {
            get
            {
                
                if (_instance == null)
                {
                    Debug.LogError("BossHealthBarUI instance is not initialized yet!");
                }
                return _instance;
            }
        }

        [SerializeField] private Slider healthSlider;
        [SerializeField] private GameObject barContainer;

        private void Awake()
        {
           
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject); 
            }
            else
            {
                _instance = this; 
            }
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
