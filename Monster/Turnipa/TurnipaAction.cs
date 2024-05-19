using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnipaAction : MonsterActionBase
{
    protected override void Awake()
    {
        key = 203;
        base.Awake();
    }

    public override void AttackRange()
    {
        Collider[] hitData = Physics.OverlapBox(transform.position + Vector3.up * 0.5f + transform.forward * 0.6f, new Vector3(0.25f, 1, 1) / 2, Quaternion.identity, 1 << 7);
        for (int i = 0; i < hitData.Length; i++)
        {
            hitData[i].GetComponent<PlayerActionBase>().Damaged(monsterInfo);
        }
    }

    public override void Damaged(PlayerInfoBase _player, float AdditionalDamage = 1)
    {
        base.Damaged(_player, AdditionalDamage);
        SoundManager.Instance.PlayMobSfx(MobSfx.MobDamage);
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.up * 0.5f + transform.forward * 0.6f, new Vector3(0.25f, 1, 1));
    }*/
}
