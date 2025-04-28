using UnityEngine;

namespace Level
{
    public class ParallaxLayer : MonoBehaviour
    {
        [SerializeField] private float parallaxMultiplier = 0.5f;
        private Vector3 previousCameraPosition;
        private Transform cameraTransform;

        private void Start()
        {
            cameraTransform = Camera.main.transform;
            previousCameraPosition = cameraTransform.position;
        }

        private void LateUpdate()
        {
            Vector3 delta = cameraTransform.position - previousCameraPosition;
            transform.position += new Vector3(delta.x * parallaxMultiplier, delta.y * parallaxMultiplier, 0);
            previousCameraPosition = cameraTransform.position;
        }
    }
}
