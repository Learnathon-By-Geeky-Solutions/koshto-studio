using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Scene
{
    public class SceneFader : MonoBehaviour
    {
        [Header("Fade Settings")]
        public CanvasGroup fadeCanvasGroup;
        public float fadeDuration = 1f;

        // Public method for fading to a specific scene
        public void FadeToScene(string sceneName)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName));
        }

        // Public coroutine for general fading (now accessible to other scripts)
        public IEnumerator Fade(float targetAlpha)
        {
            fadeCanvasGroup.blocksRaycasts = true;

            float startAlpha = fadeCanvasGroup.alpha;
            float time = 0;

            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
                yield return null;
            }

            fadeCanvasGroup.alpha = targetAlpha;
            fadeCanvasGroup.blocksRaycasts = (targetAlpha == 1f);
        }

        private IEnumerator FadeAndSwitchScenes(string sceneName)
        {
            yield return StartCoroutine(Fade(1f)); // Fade out
            SceneManager.LoadScene(sceneName);
        }

        private void Start()
        {
            // Start with fade-in
            StartCoroutine(Fade(0f));
        }
    }
}