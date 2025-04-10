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
            InitializeComponents();
            BindInputs();
        }

        private void Start() => movementSpeed = maxSpeed;

        private void Update()
        {
            UpdateGroundStatus();
            HandleMovement();
            HandleWallSlide();
            UpdateAnimations();
        }

        private void FixedUpdate() => ApplyMovement();

        private void InitializeComponents()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            controls = new PlayerControls();
        }

        private void BindInputs()
        {
            controls.Gameplay.Move.performed += ctx => moveInputX = ctx.ReadValue<Vector2>().x;
            controls.Gameplay.Move.canceled += _ => moveInputX = 0f;
            controls.Gameplay.Jump.performed += _ => HandleJump();
            controls.Gameplay.Dash.performed += _ => StartCoroutine(Dash());
            controls.Gameplay.Sprint.performed += _ => isSprinting = true;
            controls.Gameplay.Sprint.canceled += _ => isSprinting = false;
        }
    }
}