using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main_ButtonEvent : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] Image img2;
    [SerializeField] Slider slider;
    [SerializeField] PressAnyKeyText pk;

    bool hiddenMode = false;
    bool screenClick = false;

    private void Awake()
    {
        img.color = Color.clear;
        img2.color = Color.clear;
        slider.maxValue = 4f;
        slider.value = 0f;
        slider.gameObject.SetActive(false);
    }

    private void Start()
    {
        SoundManager.Instance.AllStopSFX();
        SoundManager.Instance.PlayBGM(BgmSound.MainBgm);
    }

    public void ScreenClick()
    {
        if (screenClick) return;
        screenClick = true;
        SoundManager.Instance.PlaySFX(Sfx.Portal);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float time = 0f;
        img.enabled = true;
        while (time < 1f)
        {
            yield return null;
            time += Time.deltaTime * 2;
            img.color = new Color(0, 0, 0, time / 1f);
        }
        img.color = new Color(0, 0, 0, 1);
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        img2.color = Color.white;
        slider.gameObject.SetActive(true);
        float time = 0f;

        while (time < 4f)
        {
            yield return null;
            time += Time.deltaTime;
            if (time > 3.7f)
                slider.value = 4f;
            else if (time > 2.5f)
                slider.value = 3.2f;
            else if (time > 1.7f)
                slider.value = 2.5f;
            else if (time > 1.1f)
                slider.value += Time.deltaTime * 1.5f;
            else if (time > 1f)
                slider.value = 1.6f;
            else if (time > 0.3f)
                slider.value = 0.8f;
            else if (time > 0f)
                slider.value = 0.4f;
        }

        SceneManager.LoadScene("Stage");
    }

    public void HiddenMode()
    {
        if (hiddenMode) return;
        hiddenMode = true;
        SoundManager.Instance.PlaySFX(Sfx.HiddenMode);
        StartCoroutine(pk.HiddenText());
        PlayerPrefs.SetInt("SpearFish", 1);
    }
}
