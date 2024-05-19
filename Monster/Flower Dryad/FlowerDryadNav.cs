using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMonsterState { Idle, Move, Angry, Die }

public class FlowerDryadNav: MonsterNavBase
{
    protected override void Awake()
    {
        key = 201;
        base.Awake();
    }
}
