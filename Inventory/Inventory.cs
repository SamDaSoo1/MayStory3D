using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    // ������ ������� Slot ����(���â)
    [SerializeField] List<Item> itemList;

    // ������ ������� Slot ����(�Һ�â)
    [SerializeField] List<Item> itemList2;

    // ��� Slot�� �ҷ���
    [SerializeField] Slot[] slots;

    // �Һ��� ���� ǥ���ϴ� �ؽ�Ʈ
    [SerializeField] ItemCount[] itemCount;

    [SerializeField] Button SortButton1;
    [SerializeField] Button SortButton2;

    int tabNum = 0;

    // �����ư�� �������ʾƵ� �����Ϳ��� �ٷ� ����Ǵ� �̺�Ʈ �Լ�(��.. ó�� ���� 24/03/25)
    private void OnValidate()
    {
        slots = GetComponentsInChildren<Slot>();
        itemCount = GetComponentsInChildren<ItemCount>();
        itemList = new List<Item>(new Item[slots.Length]);
        itemList2 = new List<Item>(new Item[slots.Length]);
    }

    private void Start()
    {
        FreshSlot();
    }

    private void Update()
    {
        CreateItem();
    }

    public void FreshSlot()
    {
        tabNum = 1;
        SortButton1.onClick.RemoveAllListeners();
        SortButton2.onClick.RemoveAllListeners();
        SortButton1.onClick.AddListener(SortTypeEquipment);
        SortButton2.onClick.AddListener(SortUpEquip);

        for (int i = 0; i < itemList.Count; i++)
        {
            slots[i].item = itemList[i];
        }

        for(int j = 0; j < itemCount.Length; j++)
        {
            itemCount[j].TextReset();
        }
    }

    public void FreshSlot2()
    {
        tabNum = 2;
        SortButton1.onClick.RemoveAllListeners();
        SortButton2.onClick.RemoveAllListeners();
        SortButton1.onClick.AddListener(SortTypeConsumable);
        SortButton2.onClick.AddListener(SortUpConsumable);
 
        for (int i = 0; i < itemList2.Count; i++)
        {
            slots[i].item = itemList2[i];
        }

        for (int j = 0; j < itemCount.Length; j++)
        {
            if (itemList2[j] == null)
                continue;
            itemCount[j].TextUpdate(itemList2[j].count);
        }
    }

    void AddItem(Item _item)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] == null)
            {
                itemList[i] = _item;
                FreshSlot();
                return;
            }
        }

        print("������ ���� á���ϴ�.");
    }

    void AddItem2(Item _item)
    {
        for (int i = 0; i < itemList2.Count; i++)
        {
            if (itemList2[i] == _item)
            {
                //itemList2[i] = _item;
                itemList2[i].count += 1;
                itemCount[i].TextUpdate(itemList2[i].count);
                FreshSlot2();
                return;
            }
            
        }

        for (int i = 0; i < itemList2.Count; i++)
        {
            if (itemList2[i] == null)
            {
                itemList2[i] = _item;
                itemList2[i].count = 1;
                itemCount[i].TextUpdate(itemList2[i].count);
                FreshSlot2();
                return;
            }
        }

        //print("������ ���� á���ϴ�.");
    }

    public void DropItem(int pickIdx, int dropIdx)
    {
        if (pickIdx == dropIdx) return;

        Sprite tempImg = slots[dropIdx].img.sprite;
        Color tempColor = slots[dropIdx].img.color;
        slots[dropIdx].img.sprite = slots[pickIdx].img.sprite;
        slots[dropIdx].img.color = slots[pickIdx].img.color;
        slots[pickIdx].img.sprite = tempImg;
        slots[pickIdx].img.color = tempColor;

        /// Item tempItem = itemList[pickIdx];
        /// itemList[pickIdx] = itemList[dropIdx];
        /// itemList[dropIdx] = tempItem;
        if(tabNum == 1)
            (itemList[dropIdx], itemList[pickIdx]) = (itemList[pickIdx], itemList[dropIdx]);       // Ʃ�� �ڵ���õ�ϱ淡 �ẽ. �ű�..
        else if (tabNum == 2)
        {
            (itemList2[dropIdx], itemList2[pickIdx]) = (itemList2[pickIdx], itemList2[dropIdx]);
            itemCount[dropIdx].Swap(itemCount[pickIdx]);
        }       
            

        //FreshSlot();
    }

    // ������ ����
    public void SortTypeEquipment()
    {
        print("���â ������ ����");
        var nonNullValues = itemList.Where(data => data != null).OrderBy(data => data.weight).ToList();      // �� ���� �ƴѰ� ã�� ����, weight�� �������� ������������ ������ ����Ʈ ��ȯ
        var nullValues = itemList.Where(data => data == null).ToList();                                      // �� ���� ��Ƴ��� ����Ʈ ��ȯ
        var sortedValues = nonNullValues.Concat(nullValues).ToList();                                        // nonNullValues ����Ʈ�� nullValues�� �״�� ���� ���̰� ���ο� ����Ʈ�� ��ȯ
        itemList = sortedValues;                                                                             // itemList�� ���������� ������� ����Ʈ�� �ʱ�ȭ�Ѵ�.
        FreshSlot();
    }

    // ���� ������
    public void SortUpEquip()
    {
        print("���â ���� ������");
        var nonNullValues = itemList.Where(data => data != null).ToList();
        var nullValues = itemList.Where(data => data == null).ToList();
        var sortedValues = nonNullValues.Concat(nullValues).ToList();
        itemList = sortedValues;
        FreshSlot();
    }

    public void SortTypeConsumable()
    {
        print("�Һ�â ������ ����");
        var nonNullValues = itemList2.Where(data => data != null).OrderBy(data => data.weight).ToList();      // �� ���� �ƴѰ� ã�� ����, weight�� �������� ������������ ������ ����Ʈ ��ȯ
        var nullValues = itemList2.Where(data => data == null).ToList();                                      // �� ���� ��Ƴ��� ����Ʈ ��ȯ
        var sortedValues = nonNullValues.Concat(nullValues).ToList();                                         // nonNullValues ����Ʈ�� nullValues�� �״�� ���� ���̰� ���ο� ����Ʈ�� ��ȯ
        itemList2 = sortedValues;                                                                             // itemList�� ���������� ������� ����Ʈ�� �ʱ�ȭ�Ѵ�.

        for(int i = 0; i < itemList2.Count; i++)
        {
            if (itemList2[i] == null)
            {
                itemCount[i].TextReset();
                continue;
            }
            itemCount[i].TextUpdate(itemList2[i].count);
        }
        FreshSlot2();
    }

    public void SortUpConsumable()
    {
        print("�Һ�â ���� ������");
        var nonNullValues = itemList2.Where(data => data != null).ToList();
        var nullValues = itemList2.Where(data => data == null).ToList();
        var sortedValues = nonNullValues.Concat(nullValues).ToList();
        itemList2 = sortedValues;

        for (int i = 0; i < itemList2.Count; i++)
        {
            if (itemList2[i] == null)
            {
                itemCount[i].TextReset();
                continue;
            }
            itemCount[i].TextUpdate(itemList2[i].count);
        }
        FreshSlot2();
    }

    // �ӽ÷� ����
    void CreateItem()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddItem(Resources.Load<Item>("Item/Item 1"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddItem(Resources.Load<Item>("Item/Item 2"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddItem(Resources.Load<Item>("Item/Item 3"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AddItem(Resources.Load<Item>("Item/Item 4"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AddItem(Resources.Load<Item>("Item/Item 5"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            AddItem2(Resources.Load<Item>("Item/Item 6"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            AddItem2(Resources.Load<Item>("Item/Item 7"));
        }
    }
}
