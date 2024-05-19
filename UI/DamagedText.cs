using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class DamagedText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmp;

    float time;

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        transform.GetComponent<Canvas>().worldCamera = Camera.main;
        StartCoroutine(ScaleChange());
    }

    private void Update()
    {
        time += Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime / 5, transform.position.z);

        if (time > 0.1f)
            

        if (time > 0.5f)
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 1.5f - time);

        if (time > 1.5f)
            Destroy(gameObject);
    }

    public void SetUp(int damage, Transform tr, int sorting)
    {
        GetComponent<Canvas>().sortingOrder = sorting;
        tmp.text = damage.ToString();
        transform.position = tr.position + Vector3.up;
    }

    IEnumerator ScaleChange()
    {
        tmp.rectTransform.DOScale(Vector3.one * 1.7f, 0.15f);
        yield return new WaitForSeconds(0.15f);
        tmp.rectTransform.DOScale(Vector3.one, 0.15f);
    }
}
