using UnityEngine;
using UnityEngine.UI;
using Common;

namespace Enemy
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);
        [SerializeField] private Camera mainCamera;

        private void Awake()
        {
            if (health == null)
                health = GetComponentInParent<Health>();

            if (mainCamera == null)
                mainCamera = Camera.main;

            if (health != null)
                health.OnHealthChanged += UpdateHealth;
        }

        private void LateUpdate()
        {
            if (health != null)
            {
                Vector3 screenPos = mainCamera.WorldToScreenPoint(health.transform.position + offset);
                transform.position = screenPos;
            }
        }

        private void UpdateHealth(int current, int max)
        {
            if (healthSlider != null)
                healthSlider.value = (float)current / max;
        }

        private void OnDestroy()
        {
            if (health != null)
                health.OnHealthChanged -= UpdateHealth;
        }
    }
}