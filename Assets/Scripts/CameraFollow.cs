using UnityEngine;
namespace MyGame.Camera
{


    public class CameraFollow : MonoBehaviour
    {
        public Transform player; // Assign your player in the Inspector
        public float smoothSpeed = 5f;
        public Vector3 offset = new Vector3(0, 1, -10); // Adjust the Y offset to center the player

        void LateUpdate()
        {
            if (player != null)
            {
                // Follow the player’s X and Y, but keep the Z fixed for 2D
                Vector3 targetPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, -10);
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            }
        }
    }
}
    