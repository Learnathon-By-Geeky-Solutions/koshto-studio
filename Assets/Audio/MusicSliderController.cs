using Music;
using UnityEngine;
using UnityEngine.UI;

namespace Music
{
    public class MusicVolumeControl : MonoBehaviour
    {
        [SerializeField] private Slider musicSlider;

        private void Start()
        {
            if (musicSlider == null)
                musicSlider = GetComponent<Slider>();

            // Initialize with saved volume
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);

            // Add listener
            musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        }

        private void UpdateMusicVolume(float volume)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMusicVolume(volume);
            }
        }
    }
}