using UnityEngine;
using UnityEngine.InputSystem;
using Player.Weapons;

namespace Player
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Transform weaponHolder;
        private Collider2D weaponInRange;
        private Weapon currentWeapon;

        private PlayerControls controls;

        private void Awake()
        {
            controls = new PlayerControls();
        }

        private void OnEnable()
        {
            controls.Gameplay.Enable();

            controls.Gameplay.Pickup.performed += OnPickupPerformed;
            controls.Gameplay.Attack.performed += OnAttackPerformed;
        }

        private void OnDisable()
        {
            controls.Gameplay.Pickup.performed -= OnPickupPerformed;
            controls.Gameplay.Attack.performed -= OnAttackPerformed;

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

        private void OnAttackPerformed(InputAction.CallbackContext _)
        {
            currentWeapon?.TryAttack(); // Unified attack call
        }

        private void PickupWeapon(GameObject weapon)
        {
            weapon.transform.SetParent(weaponHolder);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;

            var weaponScript = weapon.GetComponent<Weapon>(); // CHANGE: use Weapon (the base class)
            if (weaponScript != null)
            {
                weaponScript.enabled = true;
                currentWeapon = weaponScript;
            }

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
