using UnityEngine;

namespace Parallax
{
    public class ParallaxBackground : MonoBehaviour
    {
        [System.Serializable]
        public class ParallaxLayer
        {
            public Transform layerParent;
            [Range(0f, 1f)]
            public float parallaxEffect = 0.5f;
            private float spriteWidth;
            private Vector3 startPos;
            private Transform cam;

            public void Initialize(Transform cameraTransform)
            {
                cam = cameraTransform;
                startPos = layerParent.position;

                // Calculate width from first child sprite
                if (layerParent.childCount > 0)
                {
                    SpriteRenderer sr = layerParent.GetChild(0).GetComponent<SpriteRenderer>();
                    if (sr != null) spriteWidth = sr.bounds.size.x;
                }
            }

            public void UpdateLayer()
            {
                if (cam == null) return;

                float distance = cam.position.x * parallaxEffect;
                float temp = cam.position.x * (1 - parallaxEffect);

                // Move entire parent
                layerParent.position = startPos + Vector3.right * distance;

                // Infinite scroll logic
                if (temp > startPos.x + spriteWidth)
                {
                    startPos.x += spriteWidth;
                }
                else if (temp < startPos.x - spriteWidth)
                {
                    startPos.x -= spriteWidth;
                }
            }
        }

        public ParallaxLayer[] layers = new ParallaxLayer[7];
        private Transform camTransform;

        private void Start()
        {
            camTransform = Camera.main.transform;

            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].layerParent != null)
                {
                    layers[i].Initialize(camTransform);

                    // Set Z position (further back = higher Z)
                    Vector3 pos = layers[i].layerParent.position;
                    pos.z = 10 + i;
                    layers[i].layerParent.position = pos;
                }
            }
        }

        private void LateUpdate()
        {
            foreach (var layer in layers)
            {
                if (layer.layerParent != null)
                {
                    layer.UpdateLayer();
                }
            }
        }
    }
}