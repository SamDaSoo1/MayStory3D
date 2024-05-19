using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectItemImg : MonoBehaviour
{
    [SerializeField] Inventory inventory;

    [SerializeField] Button SortButton1;
    [SerializeField] Button SortButton2;
    [SerializeField] Button Tab_Equipment;
    [SerializeField] Button Tab_Consumable;

    RectTransform rt;
    public Image img;
    
    int pickIdx;
    int dropIdx;
    bool dropFail = false;

    Vector3 invenPos;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        img.color = new Color(1, 1, 1, 0);
        invenPos = inventory.gameObject.GetComponent<RectTransform>().position;
    }
    
    void Update()
    {
        rt.position = Input.mousePosition;

        ClickOutsideInventoryCheck();
    }

    public void ItemPositionControl(Image _img, int slotIdx)
    {
        if (img.color.a == 0)
        {
            SortButton1.interactable = false;
            SortButton2.interactable = false;
            Tab_Equipment.interactable = false;
            Tab_Consumable.interactable = false;

            // 아무것도 안들고 있으면 집음
            pickIdx = slotIdx;
            img.sprite = _img.sprite;
            img.color = new Color(1, 1, 1, 0.5f);       // 옮길 때 반투명
            //print(pickIdx); 
        }
        else
        {
            // 뭔가 들고있으면 놓음
            dropIdx = slotIdx;
            inventory.DropItem(pickIdx, dropIdx);
            //print(dropIdx);

            ItemDrop();
        }
    }    

    void ClickOutsideInventoryCheck()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x > invenPos.x + 360 || Input.mousePosition.x < invenPos.x - 360 || Input.mousePosition.y > invenPos.y + 440 || Input.mousePosition.y < invenPos.y - 440)
            {
                dropFail = true;
                img.color = new Color(1, 1, 1, 0);
            }
        }

        if(Input.GetMouseButtonUp(0) && dropFail)
        {
            dropFail = false;
            SortButton1.interactable = true;
            SortButton2.interactable = true;
            Tab_Equipment.interactable = true;
            Tab_Consumable.interactable = true;
        }
    }

    public void ItemDrop()
    {
        SortButton1.interactable = true;
        SortButton2.interactable = true;
        Tab_Equipment.interactable = true;
        Tab_Consumable.interactable = true;
        img.color = new Color(1, 1, 1, 0);
    }
}
