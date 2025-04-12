using UnityEngine;

namespace Common
{
    public class CheckpointManager : MonoBehaviour
    {
        public static CheckpointManager Instance { get; private set; }

        [SerializeField] private Transform defaultSpawnPoint;
        private Vector3 respawnPosition;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Use default spawn point if available
            if (defaultSpawnPoint != null)
                respawnPosition = defaultSpawnPoint.position;
            else
                respawnPosition = Vector3.zero;
        }

        public void SetCheckpoint(Vector3 position)
        {
            respawnPosition = position;
        }

        public Vector3 GetCheckpoint()
        {
            return respawnPosition;
        }

    }
}