using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossSkillEffect : MonoBehaviour
{
    [SerializeField] GameObject skill1Effect1;
    [SerializeField] GameObject skill1Effect2;
    [SerializeField] GameObject skill1Effect3;
    [SerializeField] GameObject skill2Effect;

    void Start()
    {
        skill1Effect1.SetActive(false);
        skill1Effect2.SetActive(false);
        skill1Effect3.SetActive(false);
        skill2Effect.SetActive(false);
    }

    public void Shoot(Transform boss)
    {
        if(boss.forward.x < 0)
        {
            skill1Effect1.transform.position = new Vector3(boss.position.x - 1, boss.position.y + 2, 0);
            skill1Effect1.SetActive(true);
            skill1Effect1.transform.DOMove(new Vector3(boss.position.x - 3, boss.position.y, 0), 0.5f);

            skill1Effect2.transform.position = new Vector3(boss.position.x - 1, boss.position.y + 2, 0);
            skill1Effect2.SetActive(true);
            skill1Effect2.transform.DOMove(new Vector3(boss.position.x - 2.1f, boss.position.y, 0), 0.5f);

            skill1Effect3.transform.position = new Vector3(boss.position.x - 1, boss.position.y + 2, 0);
            skill1Effect3.SetActive(true);
            skill1Effect3.transform.DOMove(new Vector3(boss.position.x - 1.2f, boss.position.y, 0), 0.5f);
        }
        else if (boss.forward.x > 0)
        {
            skill1Effect1.transform.position = new Vector3(boss.position.x + 1, boss.position.y + 2, 0);
            skill1Effect1.SetActive(true);
            skill1Effect1.transform.DOMove(new Vector3(boss.position.x + 3, boss.position.y, 0), 0.5f);

            skill1Effect2.transform.position = new Vector3(boss.position.x + 1, boss.position.y + 2, 0);
            skill1Effect2.SetActive(true);
            skill1Effect2.transform.DOMove(new Vector3(boss.position.x + 2.1f, boss.position.y, 0), 0.5f);

            skill1Effect3.transform.position = new Vector3(boss.position.x + 1, boss.position.y + 2, 0);
            skill1Effect3.SetActive(true);
            skill1Effect3.transform.DOMove(new Vector3(boss.position.x + 1.2f, boss.position.y, 0), 0.5f);
        }

        StartCoroutine(Skill1Off());
    }

    public void Scatter(Transform boss)
    {
        skill2Effect.transform.position = boss.position;
        skill2Effect.SetActive(true);
        StartCoroutine(Skill2Off());
    }

    IEnumerator Skill1Off()
    {
        yield return new WaitForSeconds(2);
        skill1Effect1.SetActive(false);
        skill1Effect2.SetActive(false);
        skill1Effect3.SetActive(false);
    }

    IEnumerator Skill2Off()
    {
        yield return new WaitForSeconds(3);
        skill2Effect.SetActive(false);
    }
}
