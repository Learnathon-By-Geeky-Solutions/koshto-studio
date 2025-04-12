using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyCore : MonoBehaviour
    {
        [SerializeField] 
        private float moveSpeed = 2f; // private field

        // Public property to access and modify moveSpeed
        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = value;
        }

        [SerializeField]
        private float detectionRange = 5f; // now private

        // Public property to access and modify detectionRange
        public float DetectionRange
        {
            get => detectionRange;
            set => detectionRange = value;
        }

        [HideInInspector] public Transform player;
        [HideInInspector] public Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }
}
