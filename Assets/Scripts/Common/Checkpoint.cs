using UnityEngine;

namespace Common
{
    public class Checkpoint : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                CheckpointManager.Instance.SetCheckpoint(transform.position);
                Debug.Log("Checkpoint reached at: " + transform.position);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.3f);
        }
    }
}