using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneEnd : MonoBehaviour
{
    [SerializeField] Image loading;

    void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.25f);
        float time = 0f;
        while (time < 1f)
        {
            yield return null;
            time += Time.deltaTime;
            loading.color = new Color(0, 0, 0, (1 - time) / 1f);
        }
        loading.color = new Color(0, 0, 0, 0);
        loading.enabled = false;
    }

    public IEnumerator FadeIn()
    {
        loading.enabled = true;
        float time = 0f;
        while (time < 1f)
        {
            yield return null;
            time += Time.deltaTime;
            loading.color = new Color(0, 0, 0, time / 1f);
        }
        loading.color = new Color(0, 0, 0, 1);
    }
}
