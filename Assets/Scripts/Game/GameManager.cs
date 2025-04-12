using UnityEngine;
using UnityEngine.SceneManagement;
using Common;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform uiCanvas;
        [SerializeField] private Transform defaultSpawnPoint;

        private GameObject currentPlayer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SetupLevel();
        }

        public void SetupLevel()
        {
            Vector3 spawnPoint = CheckpointManager.Instance.GetCheckpoint();
            if (spawnPoint == Vector3.zero && defaultSpawnPoint != null)
                spawnPoint = defaultSpawnPoint.position;

            SpawnPlayer(spawnPoint);
            SetupUI();
        }

        private void SpawnPlayer(Vector3 position)
        {
            if (playerPrefab != null)
            {
                currentPlayer = Instantiate(playerPrefab, position, Quaternion.identity);
            }
        }

        private void SetupUI()
        {
            if (uiCanvas != null)
                uiCanvas.gameObject.SetActive(true);
        }

        public void ReloadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // public void SetCheckpoint(Vector3 position)
        // {
        //     CheckpointManager.Instance.SetCheckpoint(position);
        // }
    }
}