using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyCore : MonoBehaviour
    {
        public float moveSpeed = 2f;
        public float detectionRange = 5f;

        [HideInInspector] public Transform player;
        [HideInInspector] public Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }
}