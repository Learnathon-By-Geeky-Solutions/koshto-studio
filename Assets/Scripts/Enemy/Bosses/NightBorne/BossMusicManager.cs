using UnityEngine;

namespace Enemy.Bosses.NightBorne
{
    public class BossMusicManager : MonoBehaviour
    {
        
        private static BossMusicManager _instance;
        public static BossMusicManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("BossMusicManager instance is not initialized yet!");
                }
                return _instance;
            }
        }

        [SerializeField] private AudioClip bossMusic;
        private AudioSource audioSource;

        private void OnEnable()
        {
            
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject); // Destroy duplicate instances
            }
            else
            {
                _instance = this; // Set the instance
                audioSource = GetComponent<AudioSource>();
            }
        }

        public void PlayMusic()
        {
            audioSource.clip = bossMusic;
            audioSource.loop = true;
            audioSource.Play();
        }

        public void StopMusic()
        {
            audioSource.Stop();
        }
    }
}
