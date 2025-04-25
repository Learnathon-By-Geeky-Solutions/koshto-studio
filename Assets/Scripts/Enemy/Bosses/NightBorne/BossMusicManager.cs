using UnityEngine;

namespace Enemy.Bosses.NightBorne
{
    public class BossMusicManager : MonoBehaviour
    {
        public static BossMusicManager Instance;
        [SerializeField] private AudioClip bossMusic;
        private AudioSource audioSource;

        private void Awake()
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
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