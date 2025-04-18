using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Player.Weapons;

namespace Player
{
    public class WeaponHandler : MonoBehaviour
    {
        private PlayerControls controls;
        [SerializeField] private Weapon equippedWeapon;
        [SerializeField] private Weapon primaryWeapon;
        [SerializeField] private Weapon secondaryWeapon;
        [SerializeField] private Transform weaponHolder;

        private bool isAttacking = false;

        private void Awake()
        {
            controls = new PlayerControls();

            // Manual input binding — only what WeaponHandler needs
            controls.Gameplay.PrimaryAttack.performed += OnPrimaryAttack;
            controls.Gameplay.SecondaryAttack.performed += OnSecondaryAttack;
            controls.Gameplay.Pickup.performed += OnPickup;
        }

        private void OnEnable()
        {
            controls.Gameplay.Enable();
        }

        private void OnDisable()
        {
            controls.Gameplay.Disable();
        }

        public void SetEquippedWeapon(Weapon newWeapon)
        {
            equippedWeapon = newWeapon;

            // Optional: set as primary or secondary
            if (newWeapon is MeleeWeapon)
                primaryWeapon = newWeapon;
            else
                secondaryWeapon = newWeapon;

            Debug.Log($"Equipped weapon: {newWeapon.name}");
        }

        public void FlipWeapon(bool isFacingRight)
        {
            if (equippedWeapon == null) return;

            Vector3 weaponScale = equippedWeapon.transform.localScale;
            weaponScale.x = Mathf.Abs(weaponScale.x) * (isFacingRight ? 1 : -1);
            equippedWeapon.transform.localScale = weaponScale;
        }

        public void OnPrimaryAttack(InputAction.CallbackContext context)
        {
            if (!context.performed || primaryWeapon == null || isAttacking) return;
            primaryWeapon.gameObject.SetActive(true);
            if (secondaryWeapon != null) secondaryWeapon.gameObject.SetActive(false);

            isAttacking = true;
            primaryWeapon.TryAttack();
            StartCoroutine(ResetAttack());
        }

        public void OnSecondaryAttack(InputAction.CallbackContext context)
        {
            if (!context.performed || secondaryWeapon == null || isAttacking) return;
            secondaryWeapon.gameObject.SetActive(true);
            if (primaryWeapon != null) primaryWeapon.gameObject.SetActive(false);

            isAttacking = true;
            secondaryWeapon.TryAttack();
            StartCoroutine(ResetAttack());
        }

        private IEnumerator ResetAttack()
        {
            yield return new WaitForSeconds(0.2f);
            isAttacking = false;
        }

        public void OnPickup(InputAction.CallbackContext context)
        {
            if (!context.performed || equippedWeapon == null)
            {
                return;
            }
            Debug.Log("Pickup performed!");
        }

        public void EquipWeapon(Weapon newWeapon)
        {
            if (newWeapon.weaponType == WeaponType.Primary)
            {
                if (primaryWeapon != null)
                    DropWeapon(primaryWeapon);

                primaryWeapon = newWeapon;
                newWeapon.gameObject.SetActive(true);
                if (secondaryWeapon != null)
                    secondaryWeapon.gameObject.SetActive(false);
            }
            else if (newWeapon.weaponType == WeaponType.Secondary)
            {
                if (secondaryWeapon != null)
                    DropWeapon(secondaryWeapon);

                secondaryWeapon = newWeapon;
                newWeapon.gameObject.SetActive(true);
                if (primaryWeapon != null)
                    primaryWeapon.gameObject.SetActive(false);
            }

            newWeapon.transform.SetParent(weaponHolder);
            newWeapon.transform.localPosition = Vector3.zero;
            newWeapon.transform.localRotation = Quaternion.identity;

            // Disable pickup components
            var col = newWeapon.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            var rb = newWeapon.GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = false;
        }

        private static void DropWeapon(Weapon weapon)
        {
            weapon.transform.SetParent(null);
            weapon.gameObject.SetActive(true);

            var col = weapon.GetComponent<Collider2D>();
            if (col != null) col.enabled = true;

            var rb = weapon.GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = true;
        }
    }
}
