using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DaggerInfo : PlayerInfoBase
{
    public override void Start()
    {
        data = DataManager.instance.GetPlayerData(1002);
        base.Start();
        //print($"단도 베이스: {Hp},{maxHp}");
        playerHpBar.SliderSet(gameObject);
    }

    private void OnEnable()
    {
        if (Hp != 0)
            playerHpBar.SliderSet(gameObject);
    }
}
