// Level/BossVictoryHandler.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Level
{
    public class BossVictoryHandler : MonoBehaviour
    {
        [SerializeField] private string returnSceneName = "Level1";
        [SerializeField] private Vector2 returnPosition = new Vector2(100, 0);

        public void OnBossDefeated()
        {
            LevelReturnData.PlayerReturnPosition = returnPosition;
            StartCoroutine(ReturnToLevel());
        }

        private IEnumerator ReturnToLevel()
        {
            yield return new WaitForSeconds(1.0f); // Small delay for effect
            SceneManager.LoadSceneAsync(returnSceneName);
        }
    }
}
