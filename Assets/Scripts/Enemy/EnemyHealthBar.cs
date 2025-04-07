using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private EnemyHealth enemyHealth;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0); // adjust as needed
        [SerializeField] private Camera mainCamera;

        private void Awake()
        {
            if (enemyHealth == null)
                enemyHealth = GetComponentInParent<EnemyHealth>();

            if (mainCamera == null)
                mainCamera = Camera.main;

            enemyHealth.OnHealthChanged += UpdateHealth;
        }

        private void LateUpdate()
        {
            // Keep health bar above enemy
            if (enemyHealth != null)
            {
                Vector3 screenPos = mainCamera.WorldToScreenPoint(enemyHealth.transform.position + offset);
                transform.position = screenPos;
            }
        }

        public void UpdateHealth(int current, int max)
        {
            if (healthSlider != null)
                healthSlider.value = (float)current / max;
        }

        private void OnDestroy()
        {
            if (enemyHealth != null)
                enemyHealth.OnHealthChanged -= UpdateHealth;
        }
    }
}