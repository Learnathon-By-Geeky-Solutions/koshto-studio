using UnityEngine;
using Enemy.Bosses.NightBorne; //reference NightBorneController

namespace Common
{
    public class Checkpoint : MonoBehaviour
    {
        [Header("Checkpoint Options")]
        public bool isBossCheckpoint = false;
        public NightBorneController bossToActivate;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                CheckpointManager.Instance.SetCheckpoint(transform.position);
                Debug.Log("Checkpoint reached at: " + transform.position);

                if (isBossCheckpoint && bossToActivate != null)
                {
                    bossToActivate.ActivateBoss();
                    Debug.Log("Boss activated!");
                }
            }
        }

        private void OnDrawGizmos()
        {
            DrawCheckpointGizmo(transform.position);
        }

        public static void DrawCheckpointGizmo(Vector3 position)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(position, 0.3f);
        }
    }
}
