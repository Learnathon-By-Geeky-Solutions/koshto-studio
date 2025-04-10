using UnityEngine;

namespace Player.input
{
    public partial class Player
    {
        private void Flip()
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            weaponHandler?.FlipWeapon(isFacingRight);
        }

        public void FaceDirection(bool faceRight)
        {
            if (isFacingRight != faceRight)
                Flip();
        }
    }
}