using UnityEngine;
using UnityEngine.UI;

public class GameSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private bool isMusic;
    private Audio audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
        if (isMusic)
        {
            slider.value = audioManager.music.GetComponent<AudioSource>().volume;
            slider.onValueChanged.AddListener(delegate { audioManager.MusicVolumeChange(slider.value); });
        }
        else
        {
            slider.value = audioManager.sfx.GetComponent<AudioSource>().volume;
            slider.onValueChanged.AddListener(delegate { audioManager.SfxVolumeChange(slider.value); });
        }
    }
}