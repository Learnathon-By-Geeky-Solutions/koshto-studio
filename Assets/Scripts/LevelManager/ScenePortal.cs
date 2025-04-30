using UnityEngine;
using UnityEngine.SceneManagement;
using Level;

public class ScenePortal : MonoBehaviour
{
    [Tooltip("Name of the scene to load")]
    public string targetSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Save the player's position before changing scenes
            LevelReturnData.PlayerReturnPosition = other.transform.position;

            // Load the target scene
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
