namespace Player.input
{
    public partial class Player
    {
        private float moveInputX;
        private bool isJumping, isDashing, isSprinting, isWallSliding, isFacingRight = true;

        private void OnEnable() => controls.Gameplay.Enable();
        private void OnDisable() => controls.Gameplay.Disable();
    }
}