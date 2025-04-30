using UnityEngine;

namespace Level
{
    public class PlayerReturnHandler : MonoBehaviour
    {
        private void Start()
        {
            if (LevelReturnData.PlayerReturnPosition != Vector2.zero)
            {
                transform.position = LevelReturnData.PlayerReturnPosition;
                LevelReturnData.PlayerReturnPosition = Vector2.zero;
            }
            else
            {
                GameObject spawn = GameObject.FindWithTag("PlayerSpawn");
                if (spawn != null)
                {
                    transform.position = spawn.transform.position;
                }
                else
                {
                    Debug.LogWarning("PlayerSpawn point not found!");
                }
            }
        }
    }
}
