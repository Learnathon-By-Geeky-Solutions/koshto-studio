using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

        [SerializeField] private Image fillImage;  // The bar fill image
        [SerializeField] private GameObject barBox;  // Bar container

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
            Hide();
        }

        public void Init(int maxHealth)
        {
            if (fillImage == null || barBox == null)
            {
                Debug.LogError("Assign FillImage and BarBox in inspector!");
                return;
            }

            fillImage.fillAmount = 1f;
        }

        public void UpdateHealth(int currentHealth, int maxHealth)
        {
            if (fillImage != null)
            {
                float targetFillAmount = (float)currentHealth / maxHealth;
                StartCoroutine(SmoothHealthUpdate(targetFillAmount));
            }
        }

        private IEnumerator SmoothHealthUpdate(float targetFillAmount)
        {
            float startFillAmount = fillImage.fillAmount;
            float timeElapsed = 0f;
            float duration = 0.5f;

            while (timeElapsed < duration)
            {
                fillImage.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            fillImage.fillAmount = targetFillAmount;
        }

        public void Show()
        {
            gameObject.SetActive(true); // This will enable BossHealthBarUI and its children
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
