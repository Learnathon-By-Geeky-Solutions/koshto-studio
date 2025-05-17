using Music;
using UnityEngine;

namespace Music
{
    public class MainMenuInitializer : MonoBehaviour
    {
        private void Start()
        {
            // Start playing menu music
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMenuMusic();
            }
        }
    }
}