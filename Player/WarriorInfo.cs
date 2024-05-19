using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorInfo : PlayerInfoBase
{
    public override void Start()
    {
        data = DataManager.instance.GetPlayerData(1001);
        base.Start();
        //print($"������ ���̽�: {Hp},{maxHp}");
        playerHpBar.SliderSet(gameObject);
    }

    private void OnEnable()
    {
        if (Hp != 0)
            playerHpBar.SliderSet(gameObject);
    }
}
