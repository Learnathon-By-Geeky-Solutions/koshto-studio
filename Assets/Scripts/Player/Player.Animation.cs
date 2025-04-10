using UnityEngine;
namespace Player.input
{
    public partial class Player
    {
        private void UpdateAnimations()
        {
            animator.SetBool("isRunning", Mathf.Abs(moveInputX) > 0.1f);
            animator.SetBool("isJumping", !isGrounded);
            animator.SetBool("isFalling", !isGrounded && rb.velocity.y < 0);
            animator.SetBool("isWallSliding", isWallSliding);

            if (isGrounded)
            {
                animator.ResetTrigger("Jump");
            }
        }
    }
}