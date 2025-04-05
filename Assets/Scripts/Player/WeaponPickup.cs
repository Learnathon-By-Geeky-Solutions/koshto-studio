using UnityEngine;
using Player.Weapons;
namespace Player
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Transform weaponHolder;
        [SerializeField] private KeyCode pickupKey = KeyCode.E;
        private Collider2D weaponInRange;
    
        private void Update()
        {
            if (weaponInRange != null && Input.GetKeyDown(pickupKey))
            {
                PickupWeapon(weaponInRange.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Weapon"))
            {
                weaponInRange = other;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other == weaponInRange)
            {
                weaponInRange = null;
            }
        }

        private void PickupWeapon(GameObject weapon)
        {
            weapon.transform.SetParent(weaponHolder);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;

            var weaponScript = weapon.GetComponent<MeleeWeapon>();
            if (weaponScript != null)
            {
                weaponScript.enabled = true; // activate weapon behavior
            }

            // Optional: disable collider so you don't re-trigger pickup
            var col = weapon.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
        }
        private void Start()
        {
            if (weaponHolder == null)
            {
                weaponHolder = transform.Find("WeaponHolder"); // Adjust name as needed
            }
        }
    }
}