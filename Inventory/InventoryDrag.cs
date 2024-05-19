using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryDrag : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] RectTransform panel;
    [SerializeField] SelectItemImg selectItemImg;

    private Vector2 startingPoint;
    private Vector2 moveBegin;
    private Vector2 moveOffset;

    bool isNotDrag = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        // print(eventData.position);
        if (selectItemImg.img.color.a > 0.1f)
        {
            selectItemImg.ItemDrop();
            isNotDrag = true;
        }
        else
        {
            isNotDrag = false;
            startingPoint = panel.anchoredPosition;
            moveBegin = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (selectItemImg.img.color.a > 0.1f || isNotDrag) return;
        moveOffset = eventData.position - moveBegin;
        panel.anchoredPosition = startingPoint + moveOffset;
    }
}
