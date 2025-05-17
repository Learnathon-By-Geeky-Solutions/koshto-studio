using UnityEngine;
using UnityEngine.Audio;

namespace Music
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource SFXSource;

        [Header("Audio Clips")]
        public AudioClip background;
        public AudioClip death;
        public AudioClip menuMusic; // Add your main menu music clip here

        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private string musicVolumeParam = "MusicVolume";

        private float currentMusicVolume = 0.75f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                // Initialize volume from saved preferences
                currentMusicVolume = PlayerPrefs.GetFloat(musicVolumeParam, 0.75f);
                SetMusicVolume(currentMusicVolume);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        public void PlayMenuMusic()
        {
            if (menuMusic != null)
            {
                musicSource.clip = menuMusic;
                musicSource.loop = true;
                musicSource.Play();
            }
        }

        public void PlayLevelMusic()
        {
            if (background != null)
            {
                musicSource.clip = background;
                musicSource.loop = true;
                musicSource.Play();
            }
        }

        public void PlaySFX(AudioClip clip)
        {
            SFXSource.PlayOneShot(clip);
        }

        public void SetMusicVolume(float volume)
        {
            currentMusicVolume = volume;
            // Convert linear slider value to logarithmic scale (dB)
            audioMixer.SetFloat(musicVolumeParam, Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat(musicVolumeParam, volume);
        }
    }
}