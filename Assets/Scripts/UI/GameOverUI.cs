namespace UI
{
    using UnityEngine;

    public class GameOverUI : MonoBehaviour
    {
        public GameObject panel;

        public void Show() => panel.SetActive(true);

        public void Hide() => panel.SetActive(false);
    }
}