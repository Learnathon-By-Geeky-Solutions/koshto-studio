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
                LevelReturnData.PlayerReturnPosition = Vector2.zero; // Reset after teleporting
            }
        }
    }
}
