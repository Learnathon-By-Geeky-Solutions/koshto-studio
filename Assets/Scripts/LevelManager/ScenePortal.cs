using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    public class ScenePortal : MonoBehaviour
    {
        public string targetSceneName = "BossArenaScene";

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                FindObjectOfType<SceneFader>().FadeToScene(targetSceneName);
            }
        }
    }
}
