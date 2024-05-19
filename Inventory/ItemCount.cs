using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class ItemCount : MonoBehaviour
{
    public TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = 0.ToString();
        tmp.color = new Color(1, 1, 1, 0);
    }

    public void TextReset()
    {
        tmp.color = new Color(1, 1, 1, 0);
        tmp.text = 0.ToString();
    }

    public void TextUpdate(int count)
    {
        tmp.color = new Color(1, 1, 1, 1);
        tmp.text = count.ToString();
    }

    public void Swap(ItemCount _itemCount)
    {
        Color color = _itemCount.tmp.color;
        _itemCount.tmp.color = tmp.color;
        tmp.color = color;

        string count = _itemCount.tmp.text;
        _itemCount.tmp.text = tmp.text;
        tmp.text = count;
    }
}
