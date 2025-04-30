using Common;
using UnityEngine;

namespace Traps
{
    public class LightningTrap : MonoBehaviour
    {
        [SerializeField] private int damage = 20;
        [SerializeField] private float cooldown = 2f;
        private bool canStrike = true;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (canStrike && other.CompareTag("Player"))
            {
                var damageable = other.GetComponent<IDamageable>();
                damageable?.TakeDamage(damage);
                StartCoroutine(Cooldown());
            }
        }

        private System.Collections.IEnumerator Cooldown()
        {
            canStrike = false;
            yield return new WaitForSeconds(cooldown);
            canStrike = true;
        }
    }
}