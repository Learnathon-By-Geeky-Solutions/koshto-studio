using UnityEngine;

namespace Player.input
{
    public partial class Player
    {
        private void Flip()
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            if (weaponHandler != null)
                weaponHandler.FlipWeapon(isFacingRight);
        }

        public void FaceDirection(bool faceRight)
        {
            if (isFacingRight != faceRight)
            {
                Flip();
            }
        }
    }
}