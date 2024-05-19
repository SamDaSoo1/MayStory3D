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
    // 실제로 들어있을 Slot 저장(장비창)
    [SerializeField] List<Item> itemList;

    // 실제로 들어있을 Slot 저장(소비창)
    [SerializeField] List<Item> itemList2;

    // 모든 Slot들 불러옴
    [SerializeField] Slot[] slots;

    // 소비템 개수 표시하는 텍스트
    [SerializeField] ItemCount[] itemCount;

    [SerializeField] Button SortButton1;
    [SerializeField] Button SortButton2;

    int tabNum = 0;

    // 실행버튼을 누르지않아도 에디터에서 바로 실행되는 이벤트 함수(오.. 처음 알음 24/03/25)
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

        print("슬롯이 가득 찼습니다.");
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

        //print("슬롯이 가득 찼습니다.");
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
            (itemList[dropIdx], itemList[pickIdx]) = (itemList[pickIdx], itemList[dropIdx]);       // 튜플 자동추천하길래 써봄. 신기..
        else if (tabNum == 2)
        {
            (itemList2[dropIdx], itemList2[pickIdx]) = (itemList2[pickIdx], itemList2[dropIdx]);
            itemCount[dropIdx].Swap(itemCount[pickIdx]);
        }       
            

        //FreshSlot();
    }

    // 종류별 정렬
    public void SortTypeEquipment()
    {
        print("장비창 종류별 정렬");
        var nonNullValues = itemList.Where(data => data != null).OrderBy(data => data.weight).ToList();      // 널 값이 아닌걸 찾은 다음, weight를 기준으로 오름차순으로 정렬한 리스트 반환
        var nullValues = itemList.Where(data => data == null).ToList();                                      // 널 값만 모아놓은 리스트 반환
        var sortedValues = nonNullValues.Concat(nullValues).ToList();                                        // nonNullValues 리스트에 nullValues를 그대로 갖다 붙이고 새로운 리스트로 반환
        itemList = sortedValues;                                                                             // itemList를 최종적으로 만들어진 리스트로 초기화한다.
        FreshSlot();
    }

    // 위로 모으기
    public void SortUpEquip()
    {
        print("장비창 위로 모으기");
        var nonNullValues = itemList.Where(data => data != null).ToList();
        var nullValues = itemList.Where(data => data == null).ToList();
        var sortedValues = nonNullValues.Concat(nullValues).ToList();
        itemList = sortedValues;
        FreshSlot();
    }

    public void SortTypeConsumable()
    {
        print("소비창 종류별 정렬");
        var nonNullValues = itemList2.Where(data => data != null).OrderBy(data => data.weight).ToList();      // 널 값이 아닌걸 찾은 다음, weight를 기준으로 오름차순으로 정렬한 리스트 반환
        var nullValues = itemList2.Where(data => data == null).ToList();                                      // 널 값만 모아놓은 리스트 반환
        var sortedValues = nonNullValues.Concat(nullValues).ToList();                                         // nonNullValues 리스트에 nullValues를 그대로 갖다 붙이고 새로운 리스트로 반환
        itemList2 = sortedValues;                                                                             // itemList를 최종적으로 만들어진 리스트로 초기화한다.

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
        print("소비창 위로 모으기");
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

    // 임시로 만듦
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
