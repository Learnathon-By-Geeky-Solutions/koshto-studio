using UnityEngine;
using UnityEngine.UI;
using Common;

namespace Enemy
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private Image fillImage; 
        [SerializeField] private Vector3 offset = new Vector3(0, 0.2f, 0);
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
                // Get enemy collider height
                float enemyHeight = GetComponentInParent<Collider2D>().bounds.size.y;
                Vector3 worldOffset = new Vector3(0, enemyHeight + 0f, 0);

                transform.position = health.transform.position + worldOffset;
                transform.LookAt(mainCamera.transform);
            }
        }



        private void UpdateHealth(int current, int max)
        {
            if (fillImage != null)
                fillImage.fillAmount = (float)current / max;
        }

        private void OnDestroy()
        {
            if (health != null)
                health.OnHealthChanged -= UpdateHealth;
        }
    }
}
