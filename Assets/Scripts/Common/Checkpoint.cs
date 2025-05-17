using UnityEngine;
using Enemy.Bosses.NightBorne;
namespace Common
{
    public class Checkpoint : MonoBehaviour
    {
        [Header("Checkpoint Options")]
        [SerializeField] private bool isBossCheckpoint = false;
        [SerializeField] private NightBorneController bossToActivate;

        public bool IsBossCheckpoint => isBossCheckpoint;
        public NightBorneController BossToActivate => bossToActivate;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                CheckpointManager.Instance.SetCheckpoint(transform.position);
                Debug.Log("Checkpoint reached at: " + transform.position);

                if (isBossCheckpoint && bossToActivate != null)
                {
                    bossToActivate.ActivateBoss();
                    BossHealthBarUI.Instance.Show();
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
