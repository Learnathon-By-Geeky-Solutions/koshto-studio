// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Player
// {
//     public class PlayerHealthBar : MonoBehaviour
//     {
//         [SerializeField] private PlayerHealth playerHealth;
//         [SerializeField] private Slider healthSlider;
//         [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);
//         [SerializeField] private Camera mainCamera;
//
//         private void Awake()
//         {
//             if (mainCamera == null)
//                 mainCamera = Camera.main;
//
//             if (playerHealth == null)
//                 playerHealth = GetComponentInParent<PlayerHealth>();
//
//             playerHealth.OnHealthChanged += UpdateHealth;
//         }
//
//         private void LateUpdate()
//         {
//             if (playerHealth != null)
//             {
//                 Vector3 screenPos = mainCamera.WorldToScreenPoint(playerHealth.transform.position + offset);
//                 transform.position = screenPos;
//             }
//         }
//
//         private void UpdateHealth(int current, int max)
//         {
//             healthSlider.value = (float)current / max;
//         }
//
//         private void OnDestroy()
//         {
//             if (playerHealth != null)
//                 playerHealth.OnHealthChanged -= UpdateHealth;
//         }
//     }
// }