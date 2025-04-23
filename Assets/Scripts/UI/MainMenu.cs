using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class MainMenu : MonoBehaviour
    {
        /// <summary>
        /// Static method to start the game by loading the next scene.
        /// Cannot be directly called from Unity UI buttons via Inspector.
        /// </summary>
        public static void PlayGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        /// <summary>
        /// Wrapper to call the static PlayGame method from UI Button.
        /// </summary>
        public void PlayGameFromButton()
        {
            PlayGame();
        }

        /// <summary>
        /// Quits the application. Works in builds only.
        /// </summary>
        public static void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
