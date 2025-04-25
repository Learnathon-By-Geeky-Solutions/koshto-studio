using UnityEngine;
using UnityEngine.UI;
using Common;
using Player.Input;

namespace UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthFill;

        private void Start()
        {
            var player = FindObjectOfType<Player.Input.PlayerController>();


            if (player == null) return;

            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth == null) return;

            playerHealth.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
        }

        private void UpdateHealthBar(int current, int max)
        {
            float fillAmount = (float)current / max;
            healthFill.fillAmount = fillAmount;
        }
    }
}
