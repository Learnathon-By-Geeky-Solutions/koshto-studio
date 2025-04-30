using UnityEngine;
using UnityEngine.SceneManagement;
using Level;

public class ScenePortal : MonoBehaviour
{
    public string targetSceneName = "Level1";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // You can remove or comment out this line:
            // LevelReturnData.PlayerReturnPosition = other.transform.position;

            SceneManager.LoadScene(targetSceneName);
        }
    }
}
