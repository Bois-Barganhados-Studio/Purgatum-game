using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public GameObject gameObjWithAudio;
    private Slider slider;
    private SoundControl soundController;

    private void Start()
    {
        slider = GetComponent<Slider>();
        soundController = GameObject.FindObjectOfType<SoundControl>();
        slider.value = SoundControl.globalSoundVolume;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        value = Mathf.Clamp01(value);
        soundController.SetGlobalSoundVolume(value);
    }
}
