using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PressAnyKeyText : MonoBehaviour
{
    [SerializeField] Image tsg;

    private void Awake()
    {
        tsg = GetComponent<Image>();
    }

    void Start()
    {
        StartCoroutine(AlphaValue_FadeInOut());
    }

    IEnumerator AlphaValue_FadeInOut()
    {
        float time = 0f;

        while(time < 1f)
        {
            yield return null;
            time += Time.deltaTime * 1.666f;
            tsg.color = new Color(tsg.color.r, tsg.color.g, tsg.color.b, 1 - time);
        }
        tsg.color = new Color(tsg.color.r, tsg.color.g, tsg.color.b, 0);
        time = 0f;
        while (time < 1f)
        {
            yield return null;
            time += Time.deltaTime * 1.666f;
            tsg.color = new Color(tsg.color.r, tsg.color.g, tsg.color.b, time);
        }
        tsg.color = new Color(tsg.color.r, tsg.color.g, tsg.color.b, 1);

        StartCoroutine(AlphaValue_FadeInOut());
    }

    public IEnumerator HiddenText()
    {
        float time = 0f;
        while (time < 0.5f)
        {
            yield return null;
            time += Time.deltaTime;
            tsg.color = new Color(tsg.color.r, tsg.color.g - time, tsg.color.b - time);
        }
        tsg.color = Color.red;
    }
}
