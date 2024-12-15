using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class _Script_MainMenuController : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        masterSlider.value = PlayerPrefs.GetFloat("masterVolume", 1);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1);
        mixer.SetFloat("Master", Mathf.Log10(masterSlider.value) * 20);
        mixer.SetFloat("Music", Mathf.Log10(musicSlider.value) * 20);
        mixer.SetFloat("SFX", Mathf.Log10(sfxSlider.value) * 20);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Floorplan Testing");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TitleScreen()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void SetMasterVolume() 
    {
        float masterVolume = masterSlider.value;
        mixer.SetFloat("Master", Mathf.Log10(masterVolume)*20);
        PlayerPrefs.SetFloat("masterVolume", masterSlider.value);
    }

    public void SetMusicVolume()
    {
        float musicVolume = musicSlider.value;
        mixer.SetFloat("Music", Mathf.Log10(musicVolume) * 20);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
    }

    public void SetSFXVolume()
    {
        float sfxVolume = sfxSlider.value;
        mixer.SetFloat("SFX", Mathf.Log10(sfxVolume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
    }
}