using System;

namespace Game
{
    public static class PlayerEvents
    {
        public static event Action OnPlayerDeath;

        public static void InvokePlayerDeath()
        {
            OnPlayerDeath?.Invoke();
        }
    }
}