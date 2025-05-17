using UnityEngine;

namespace Level
{
    public class PlayerReturnHandler : MonoBehaviour
    {
        private void Start()
        {
            // Only needed to load a new scene
            //if (LevelReturnData.PlayerReturnPosition != Vector2.zero)
            //{
            //    transform.position = LevelReturnData.PlayerReturnPosition;
            //    LevelReturnData.PlayerReturnPosition = Vector2.zero;
            //}
            //else
            //{
            //    GameObject spawn = GameObject.FindWithTag("PlayerSpawn");
            //    if (spawn != null) transform.position = spawn.transform.position;
            //}
        }
    }
}