// Game/InputManager.cs
using UnityEngine;
using System;

namespace Game
{
    public class InputManager : MonoBehaviour
    {
        private PlayerControls controls;
        private InputManager inputManager;

        public Vector2 MoveInput { get; private set; }
        public bool IsSprinting { get; private set; }

        public event Action OnJump;
        public event Action OnDash;

        private void Awake()
        {
            controls = new PlayerControls();
            inputManager = FindObjectOfType<InputManager>();

            controls.Gameplay.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
            controls.Gameplay.Move.canceled += _ => MoveInput = Vector2.zero;

            controls.Gameplay.Jump.performed += _ => OnJump?.Invoke();
            controls.Gameplay.Dash.performed += _ => OnDash?.Invoke();

            controls.Gameplay.Sprint.performed += _ => IsSprinting = true;
            controls.Gameplay.Sprint.canceled += _ => IsSprinting = false;
        }
        
        public void EnablePlayerInput()
        {
            controls.Gameplay.Enable();
        }

        public void DisablePlayerInput()
        {
            controls.Gameplay.Disable();
        }

        private void OnEnable() => controls.Gameplay.Enable();
        private void OnDisable() => controls.Gameplay.Disable();
    }
}