using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class Slot : MonoBehaviour, IPointerDownHandler
{
    public RectTransform rt;
    public Image img;
    [SerializeField] SelectItemImg selectItemImg;
    [SerializeField] int slotIdx;

    Item _item;
    public Item item
    {
        get { return _item; }

        set
        {
            _item = value;

            if (_item != null)
            {
                img.sprite = _item.itemImage;
                img.color = new Color(1, 1, 1, 1);
            }
            else if (_item == null)
            {
                img.color = new Color(1, 1, 1, 0);
            }
        }

    }

    public int count;

    private void Awake()
    {
        count = 1;
        rt = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        //tmp.text = count.ToString();
        //tmp.color = new Color(1, 1, 1, 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(img.color.a == 0 && selectItemImg.img.color.a == 0) { return; }
        selectItemImg.ItemPositionControl(img, slotIdx);
    }
}
