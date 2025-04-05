using UnityEngine;
using UnityEngine.InputSystem;
using Player.Weapons;

namespace Player
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Transform weaponHolder; // Reference to the weapon holder
        private Collider2D weaponInRange;

        // Reference the PlayerControls class
        private PlayerControls controls;

        private void Awake()
        {
            // Initialize PlayerControls
            controls = new PlayerControls();
        }

        private void OnEnable()
        {
            // Enable the Gameplay action map
            controls.Gameplay.Enable();

            // Subscribe to the Pickup action
            controls.Gameplay.Pickup.performed += OnPickupPerformed;
        }

        private void OnDisable()
        {
            // Unsubscribe from the Pickup action
            controls.Gameplay.Pickup.performed -= OnPickupPerformed;

            // Disable the Gameplay action map
            controls.Gameplay.Disable();
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

        private void OnPickupPerformed(InputAction.CallbackContext context)
        {
            if (weaponInRange != null)
            {
                PickupWeapon(weaponInRange.gameObject);
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
                weaponScript.enabled = true; // Activate weapon behavior
            }

            // Optional: disable collider so you don't re-trigger pickup
            var col = weapon.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
        }

        private void Start()
        {
            if (weaponHolder == null)
            {
                weaponHolder = transform.Find("WeaponHolder");
            }
        }
    }
}