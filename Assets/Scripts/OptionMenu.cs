using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public GameObject OptionsPanel;
    public Slider VolumeSlider;
    public Dropdown WindowDropdown;

    public void OpenOptions()
    {
        OptionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        OptionsPanel.SetActive(false);
    }

    void Start()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("Volume", 50.0f);
        WindowDropdown.value = PlayerPrefs.GetInt("Window", 1);

        VolumeSlider.onValueChanged.AddListener(SetVolume);
        WindowDropdown.onValueChanged.AddListener(SetWindow);
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        AudioListener.volume = volume;
    }

    public void SetWindow(int mode)
    {
        PlayerPrefs.SetInt("Window", mode);
        switch (mode)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            default:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

}