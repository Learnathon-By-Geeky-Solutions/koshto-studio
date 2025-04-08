using UnityEngine;
using UnityEngine.InputSystem;
using Player.Weapons;

namespace Player
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Transform weaponHolder; // Reference to the weapon holder
        private Collider2D weaponInRange;

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

        private void OnPickupPerformed(InputAction.CallbackContext _)
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

            bool isFacingRight = transform.localScale.x > 0f;

            // Flip the weapon to match player direction
            Vector3 scale = weapon.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (isFacingRight ? 1 : -1);
            weapon.transform.localScale = scale;

            // Adjust rotation if needed (optional)
            weapon.transform.localRotation = Quaternion.Euler(0, isFacingRight ? 0f : 180f, 0f);

            var weaponScript = weapon.GetComponent<Weapon>();
            if (weaponScript != null)
            {
                weaponScript.enabled = true;

                var handler = GetComponent<WeaponHandler>();
                if (handler != null)
                {
                    handler.SetEquippedWeapon(weaponScript);
                    handler.FlipWeapon(isFacingRight);
                }

                // Call firePoint flip (now called safely after weapon scale is set)
                weaponScript.FlipFirePoint(isFacingRight);
            }

            // Disable collider so it doesn’t trigger again
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