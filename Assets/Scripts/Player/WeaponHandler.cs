using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Player.input;
using Player.Weapons;

namespace Player
{
    public class WeaponHandler : MonoBehaviour, PlayerControls.IGameplayActions
    {
        private PlayerControls controls;
        [SerializeField]
        private Weapon equippedWeapon;
        [SerializeField] private Weapon primaryWeapon;
        [SerializeField] private Weapon secondaryWeapon;
        private bool isAttacking = false;


        private void Awake()
        {
            controls = new PlayerControls();
            controls.Gameplay.SetCallbacks(this);
        }

        private void OnEnable()
        {
            controls.Gameplay.Enable();
        }

        private void OnDisable()
        {
            controls.Gameplay.Disable();
        }

        public void OnMove(InputAction.CallbackContext context) { /* ignored */ }
        public void OnJump(InputAction.CallbackContext context) { /* ignored */ }
        public void OnDash(InputAction.CallbackContext context) { /* ignored */ }
        public void OnSprint(InputAction.CallbackContext context) { /* ignored */ }

        // 👇 Add this to PlayerControls.inputactions first!
        public void OnPrimaryAttack(InputAction.CallbackContext context)
        {
            if (!context.performed || primaryWeapon == null || isAttacking) return;
            isAttacking = true;
            primaryWeapon.TryAttack();
            StartCoroutine(ResetAttack());
        }

        public void OnSecondaryAttack(InputAction.CallbackContext context)
        {
            if (!context.performed || secondaryWeapon == null || isAttacking) return;
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
            // Add logic for picking up a weapon here
        }
    }
}