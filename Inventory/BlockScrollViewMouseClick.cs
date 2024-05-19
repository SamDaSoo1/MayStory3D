using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockScrollViewMouseClick : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] SelectItemImg selectItemImg;

    public void OnDrag(PointerEventData eventData)
    {
        eventData.pointerDrag = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (selectItemImg.img.color.a > 0.1f)
            selectItemImg.ItemDrop();
    }
}
