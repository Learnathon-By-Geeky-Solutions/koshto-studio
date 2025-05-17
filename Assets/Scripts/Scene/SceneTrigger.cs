using UnityEngine;
using MyGame.Camera;

namespace Scene
{
    public class SceneTrigger : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private Transform cameraTargetPosition;
        [SerializeField] private float cutsceneDuration = 3f;
        [SerializeField] private bool pauseGameDuringCutscene = true;
        [SerializeField] private bool disableAfterUse = true;

        private CameraFollow _cameraFollow;
        private bool _isCutsceneActive;

        private void Awake()
        {
            _cameraFollow = FindObjectOfType<CameraFollow>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isCutsceneActive && other.CompareTag("Player"))
            {
                StartCutscene();
            }
        }

        private void StartCutscene()
        {
            _isCutsceneActive = true;
            _cameraFollow.StartCutscene(cameraTargetPosition, cutsceneDuration);

            if (pauseGameDuringCutscene)
            {
                Time.timeScale = 0f;
            }

            if (disableAfterUse)
            {
                GetComponent<Collider2D>().enabled = false;
            }
        }

        private void OnDestroy()
        {
            if (_isCutsceneActive && pauseGameDuringCutscene)
            {
                Time.timeScale = 1f;
            }
        }
    }
}