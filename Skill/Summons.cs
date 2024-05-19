using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Summons : MonoBehaviour
{
    [SerializeField] GameObject[] players;
    [SerializeField] GameObject[] summons;
    [SerializeField] MonsterHpBar monsterHpBar;
    [SerializeField] GameObject particle;

    private void Start()
    {
        monsterHpBar = GameObject.FindObjectOfType<MonsterHpBar>();
        particle.SetActive(false);
    }

    public void Create(Transform boss)
    {
        if (players[0].activeSelf)
            particle.transform.position = players[0].transform.position - players[0].transform.forward;
        else if (players[1].activeSelf)
            particle.transform.position = players[1].transform.position - players[1].transform.forward;
        particle.SetActive(true);
        particle.GetComponent<ParticleSystem>().Play();
        int num = Random.Range(0, summons.Length);
        GameObject go = Instantiate(summons[num]);
        go.GetComponent<MonsterNavBase>().PlayerSetting(players);
        go.GetComponent<MonsterActionBase>().PlayerSetting(players);
        monsterHpBar.Setting(go);
        go.transform.position = boss.position + boss.transform.forward * 5f;
        go.GetComponent<MonsterNavBase>().posTarget = go.transform.position;
        go.GetComponent<MonsterNavBase>().center = go.transform.position;
        go.GetComponent<MonsterNavBase>().monsterState = eMonsterState.Angry;
        go.GetComponent<MonsterActionBase>().monsterState = eMonsterState.Angry;
    }
}
