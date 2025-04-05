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
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!context.performed || equippedWeapon == null)
            {
                return;
            }
            Debug.Log("Attack!");
            equippedWeapon.TryAttack();
        }
    }
}