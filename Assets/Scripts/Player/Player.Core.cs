using UnityEngine;

namespace Player.input
{
    public partial class Player : MonoBehaviour
    {
        
        private Rigidbody2D rb;
        private Animator animator;
        private PlayerControls controls;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            controls = new PlayerControls();

            // Input Events
            controls.Gameplay.Move.performed += ctx => moveInputX = ctx.ReadValue<Vector2>().x;
            controls.Gameplay.Move.canceled += ctx => moveInputX = 0f;
            controls.Gameplay.Jump.performed += ctx => HandleJump();
            controls.Gameplay.Dash.performed += ctx => StartCoroutine(Dash());
            controls.Gameplay.Sprint.performed += ctx => isSprinting = true;
            controls.Gameplay.Sprint.canceled += ctx => isSprinting = false;
        }

        private void Start()
        {
            movementSpeed = maxSpeed;
        }

        private void Update()
        {
            CheckGroundStatus();
            HandleMovement();
            HandleWallSlide();
            UpdateAnimations();
        }

        private void FixedUpdate()
        {
            ApplyMovement();
        }
    }
}