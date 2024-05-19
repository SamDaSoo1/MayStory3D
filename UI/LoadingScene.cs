using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] Slider slider;

    void Start()
    {
        slider.maxValue = 4f;
        slider.value = 0f;
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        float time = 0f;

        while(time < 4f)
        {
            yield return null;
            time += Time.deltaTime;
            if (time > 3.7f)
                slider.value = 4f;
            else if (time > 2.5f)
                slider.value = 3.7f;
            else if (time > 1.7f)
                slider.value = 2.5f;
            else if (time > 1.1f)
                slider.value += Time.deltaTime * 1.5f;
            else if (time > 1f)
                slider.value = 1.6f;
            else if (time > 0.3f)
                slider.value = 1f;
            else if (time > 0f)
                slider.value = 0.7f;
        }

        SceneManager.LoadScene("Boss Stage");
    }
}
