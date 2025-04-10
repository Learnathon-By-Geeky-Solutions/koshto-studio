using UnityEngine;

namespace Player.input
{
    public partial class Player
    {
        [Header("Sprint Settings")]
        [SerializeField] private float sprintSpeed = 10f;
        [Header("Movement Settings")]
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float acceleration = 8f;
        [SerializeField] private float deceleration = 6f;

        public float MaxSpeed => maxSpeed;
        public float Acceleration => acceleration;
        public float Deceleration => deceleration;
        private float currentSpeed;

        public float movementSpeed
        {
            get { return sprintSpeed; }
            private set { sprintSpeed = value; }
        }

        private void HandleMovement()
        {
            currentSpeed = isSprinting ? sprintSpeed : maxSpeed;

            if (moveInputX > 0 && !isFacingRight)
                Flip();
            else if (moveInputX < 0 && isFacingRight)
                Flip();
        }

        private void ApplyMovement()
        {
            float targetSpeed = moveInputX * currentSpeed;
            float speedDifference = targetSpeed - rb.velocity.x;
            float accelerationRate = (Mathf.Abs(targetSpeed) > 0.1f) ? acceleration : deceleration;
            rb.AddForce(new Vector2(speedDifference * accelerationRate, 0), ForceMode2D.Force);
        }
    }
}