using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].height == Screen.currentResolution.height && resolutions[i].width == Screen.currentResolution.width)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }

    public void masterVolume(float volume)
    {
        audioMixer.SetFloat("MastVol", volume);
    }

    public void SFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVol", volume);
    }

    public void dialogVolume(float volume)
    {
        audioMixer.SetFloat("Dialog", volume);
    }

    public void musicVolume(float volume)
    {
        audioMixer.SetFloat("Music", volume);
    }

    public void enableSubtitles(bool enable)
    {
        GameManager.Instance.subtitles = enable;
    }

    public void resolution(int index)
    {
        print("Change Resolution " + index);
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void qualitySettings(int index)
    {
        print("Change Quality " + index);
        QualitySettings.SetQualityLevel(index);
    }

    public void VsyncEnabled()
    {
       
    }

    public void motionBlur()
    {
        
    }

    public void brightness(float value)
    {
        Screen.brightness = value;
    }
}
