using UnityEngine;

namespace MyGame.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform player; // Assign your player in the Inspector

        [SerializeField]
        private float smoothSpeed = 5f;

        [SerializeField]
        private Vector3 offset = new Vector3(0, 1, -10); // Adjust the Y offset to center the player

        public Transform Player
        {
            get => player;
            set => player = value;
        }

        public float SmoothSpeed
        {
            get => smoothSpeed;
            set => smoothSpeed = value;
        }

        public Vector3 Offset
        {
            get => offset;
            set => offset = value;
        }

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
