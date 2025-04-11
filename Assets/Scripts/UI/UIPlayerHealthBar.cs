// using UnityEngine;
// using UnityEngine.UI;
//
// namespace UI
// {
//     public class UIPlayerHealthBar : MonoBehaviour
//     {
//         [SerializeField] private Slider healthSlider;
//         [SerializeField] private Player.PlayerHealth playerHealth;
//
//         private void Start()
//         {
//             if (playerHealth == null)
//                 playerHealth = FindObjectOfType<Player.PlayerHealth>();
//
//             if (playerHealth != null)
//             {
//                 playerHealth.OnHealthChanged += UpdateUI;
//                 UpdateUI(playerHealth.Current, playerHealth.Max);
//             }
//         }
//
//         private void UpdateUI(int current, int max)
//         {
//             healthSlider.value = (float)current / max;
//         }
//
//         private void OnDestroy()
//         {
//             if (playerHealth != null)
//                 playerHealth.OnHealthChanged -= UpdateUI;
//         }
//     }
// }