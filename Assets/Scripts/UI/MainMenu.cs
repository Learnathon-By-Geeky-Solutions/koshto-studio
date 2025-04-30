using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class MainMenu : MonoBehaviour
    {
   
        public static void PlayGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }

        public static void PlayGameFromButton()
        {
            PlayGame();
        }

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
