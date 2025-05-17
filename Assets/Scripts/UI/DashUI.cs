using UnityEngine;
using UnityEngine.UI;
using Player.Input;
using Player;

namespace UI
{
    public class DashUI : MonoBehaviour
    {
        [Header("Dash Charge Images")]
        [SerializeField] private Image[] dashChargeBars; // Assign 3 UI Images in Inspector
        [SerializeField] private Color activeColor = Color.white;
        [SerializeField] private Color inactiveColor = new Color(1, 1, 1, 0.2f);

        private int currentCharges;
        private int maxCharges = 3;

        private void Awake()
        {
            // Initialize all bars to active
            UpdateDashUI(maxCharges, maxCharges);
        }

        public void UpdateDashUI(int currentCharges, int maxCharges = 3)
        {
            for (int i = 0; i < dashChargeBars.Length; i++)
            {
                // Enable/disable bars based on available charges
                if (i < maxCharges)
                {
                    dashChargeBars[i].gameObject.SetActive(true);
                    dashChargeBars[i].color = i < currentCharges ? activeColor : inactiveColor;
                }
                else
                {
                    dashChargeBars[i].gameObject.SetActive(false);
                }
            }
        }
    }
}