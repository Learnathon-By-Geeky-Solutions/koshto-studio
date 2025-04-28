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

        [SerializeField] private Image fillImage;  // Image that fills the health bar
        [SerializeField] private GameObject barContainer;
        [SerializeField] private Camera mainCamera;

        private void Awake()
        {
            // Singleton pattern to ensure only one instance of BossHealthBarUI exists
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }

            if (mainCamera == null)
                mainCamera = Camera.main; // Set the main camera if not assigned
        }

        public void Init(int maxHealth)
        {
            // Check if fillImage and barContainer are assigned
            if (fillImage == null)
            {
                Debug.LogError("FillImage is not assigned in BossHealthBarUI!");
                return;
            }
            if (barContainer == null)
            {
                Debug.LogError("BarContainer is not assigned in BossHealthBarUI!");
                return;
            }

            // Set the initial fill to 1 (full health)
            fillImage.fillAmount = 1f;

            // Show the health bar container
            barContainer.SetActive(true);
        }

        public void UpdateHealth(int currentHealth, int maxHealth)
        {
            if (fillImage != null)
            {
                float targetFillAmount = (float)currentHealth / maxHealth;
                Debug.Log($"Updating Health: {currentHealth}/{maxHealth}, Fill Amount: {targetFillAmount}");
                StartCoroutine(SmoothHealthUpdate(targetFillAmount));
            }
        }

        // Coroutine to smoothly transition the health bar fill over time
        private IEnumerator SmoothHealthUpdate(float targetFillAmount)
        {
            float startFillAmount = fillImage.fillAmount;
            float timeElapsed = 0f;
            float duration = 0.5f;  // Duration of the smooth transition

            while (timeElapsed < duration)
            {
                fillImage.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            fillImage.fillAmount = targetFillAmount;  // Ensure the final fill is exactly the target value
        }

        public void Hide()
        {
            // Hide the health bar container when the boss dies
            barContainer.SetActive(false);
        }

        // Update the position of the health bar above the boss (if needed)
        private void LateUpdate()
        {
            if (fillImage != null)
            {
                Vector3 screenPosition = mainCamera.WorldToScreenPoint(barContainer.transform.position);
                barContainer.transform.position = screenPosition;
            }
        }
    }
}
