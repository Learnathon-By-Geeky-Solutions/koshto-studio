using UnityEngine;

namespace UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        public void Show() => panel.SetActive(true);
        public void Hide() => panel.SetActive(false);
    }
}