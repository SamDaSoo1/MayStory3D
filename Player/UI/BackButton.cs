using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    [SerializeField] Image blocking;
    [SerializeField] GameObject PausedWindow;
    [SerializeField] GameObject OptionsWindow;
    [SerializeField] Image musicBar;
    [SerializeField] Image soundBar;
    [SerializeField] SoundManager soundManager;

    private void Awake()
    {
        blocking.enabled = false;
    }

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();

        PausedWindow = GameObject.Find("Canvas 2").transform.GetChild(11).gameObject;
        OptionsWindow = GameObject.Find("Canvas 2").transform.GetChild(12).gameObject;
        musicBar = OptionsWindow.transform.GetChild(1).transform.GetChild(2).GetComponent<Image>();
        soundBar = OptionsWindow.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>();

        PausedWindow.SetActive(false);
        OptionsWindow.SetActive(false);

        if (PlayerPrefs.HasKey("Music") == false)
            PlayerPrefs.SetInt("Music", 50);
        if (PlayerPrefs.HasKey("Sound") == false)
            PlayerPrefs.SetInt("Sound", 50);

        musicBar.fillAmount = PlayerPrefs.GetInt("Music") / 100f;
        soundBar.fillAmount = PlayerPrefs.GetInt("Sound") / 100f;
        SoundManager.Instance.BgmVolumeSettimg(musicBar.fillAmount);
        SoundManager.Instance.SfxVolumeSettimg(soundBar.fillAmount);
    }

    public void BackButtonClick()
    {
        SoundManager.Instance.PlaySFX(Sfx.BtMouseClick);

        blocking.enabled = true;
        PausedWindow.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PlayClick()
    {
        SoundManager.Instance.PlaySFX(Sfx.BtMouseClick);

        blocking.enabled = false;
        PausedWindow.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OptionClick()
    {
        SoundManager.Instance.PlaySFX(Sfx.BtMouseClick);

        OptionsWindow.SetActive(true);
    }

    public void  QuitClick()
    {
        SoundManager.Instance.PlaySFX(Sfx.BtMouseClick);

        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }

    public void XClick()
    {
        SoundManager.Instance.PlaySFX(Sfx.BtMouseClick);

        blocking.enabled = false;
        PausedWindow.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MusicMinusClick()
    {
        SoundManager.Instance.PlaySFX(Sfx.BtMouseClick);

        int musicVolume = PlayerPrefs.GetInt("Music") - 5;
        if(musicVolume < 0) { musicVolume = 0; }
        PlayerPrefs.SetInt("Music", musicVolume);
        musicBar.fillAmount = PlayerPrefs.GetInt("Music") / 100f;
        SoundManager.Instance.BgmVolumeSettimg(musicBar.fillAmount);
    }

    public void MusicPlusClick()
    {
        SoundManager.Instance.PlaySFX(Sfx.BtMouseClick);

        int musicVolume = PlayerPrefs.GetInt("Music") + 5;
        if (musicVolume > 100) { musicVolume = 100; }
        PlayerPrefs.SetInt("Music", musicVolume);
        musicBar.fillAmount = PlayerPrefs.GetInt("Music") / 100f;
        SoundManager.Instance.BgmVolumeSettimg(musicBar.fillAmount);
    }

    public void SoundMinusClick()
    {
        SoundManager.Instance.PlaySFX(Sfx.BtMouseClick);

        int soundVolume = PlayerPrefs.GetInt("Sound") - 5;
        if (soundVolume < 0) { soundVolume = 0; }
        PlayerPrefs.SetInt("Sound", soundVolume);
        soundBar.fillAmount = PlayerPrefs.GetInt("Sound") / 100f;
        SoundManager.Instance.SfxVolumeSettimg(soundBar.fillAmount);
    }

    public void SoundPlusClick()
    {
        SoundManager.Instance.PlaySFX(Sfx.BtMouseClick);

        int soundVolume = PlayerPrefs.GetInt("Sound") + 5;
        if (soundVolume > 100) { soundVolume = 100; }
        PlayerPrefs.SetInt("Sound", soundVolume);
        soundBar.fillAmount = PlayerPrefs.GetInt("Sound") / 100f;
        SoundManager.Instance.SfxVolumeSettimg(soundBar.fillAmount);
    }

    public void XClick2()
    {
        SoundManager.Instance.PlaySFX(Sfx.BtMouseClick);

        OptionsWindow.SetActive(false);
    }
}
