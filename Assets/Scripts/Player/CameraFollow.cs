using UnityEngine;
using UnityEngine.InputSystem;

namespace MyGame.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Follow Settings")]
        [SerializeField] private Transform player;
        [SerializeField] private float smoothSpeed = 5f;
        [SerializeField] private Vector3 offset = new Vector3(0, 1, -10);

        [Header("Cutscene Settings")]
        [SerializeField] private float cutsceneDuration = 3f;
        private Transform _cutsceneTarget;
        private bool _inCutscene;
        private Vector3 _originalOffset;
        private bool _skipRequested;
        private PlayerControls _playerControls;

        private void Awake()
        {
            _originalOffset = offset;
            _playerControls = new PlayerControls();
            _playerControls.Gameplay.Jump.performed += _ => SkipCutscene();
        }

        void LateUpdate()
        {
            if (_inCutscene && _cutsceneTarget != null && !_skipRequested)
            {
                // Move to cutscene target
                Vector3 targetPosition = _cutsceneTarget.position + _originalOffset;
                transform.position = Vector3.Lerp(
                    transform.position,
                    targetPosition,
                    smoothSpeed * Time.unscaledDeltaTime
                );
            }
            else if (player != null)
            {
                // Return to player follow
                Vector3 targetPosition = player.position + _originalOffset;
                transform.position = Vector3.Lerp(
                    transform.position,
                    targetPosition,
                    smoothSpeed * Time.deltaTime
                );
            }
        }

        public void StartCutscene(Transform target, float duration)
        {
            _inCutscene = true;
            _cutsceneTarget = target;
            _skipRequested = false;
            cutsceneDuration = duration;
            _playerControls.Gameplay.Enable();
            Invoke(nameof(EndCutscene), cutsceneDuration);
        }

        private void SkipCutscene()
        {
            if (_inCutscene) EndCutscene();
        }

        public void EndCutscene()
        {
            _inCutscene = false;
            _cutsceneTarget = null;
            _playerControls.Gameplay.Disable();
            CancelInvoke(nameof(EndCutscene));
        }

        private void OnDestroy()
        {
            _playerControls.Dispose();
        }
    }
}