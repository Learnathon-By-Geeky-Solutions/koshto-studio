// Level/BossPortalCheckpoint.cs
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level
{
    public class BossPortalCheckpoint : MonoBehaviour
    {
        [SerializeField] private string bossSceneName = "BossArenaScene";

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;

            LoadBossScene();
        }

        private void LoadBossScene()
        {
            SceneManager.LoadSceneAsync(bossSceneName);
        }
    }
}
