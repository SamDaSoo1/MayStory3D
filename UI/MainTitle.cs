using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTitle : MonoBehaviour
{
    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(0, 850, 0);
        rectTransform.DOMoveY(754, 1.5f).SetEase(Ease.OutBounce);
    }
}
