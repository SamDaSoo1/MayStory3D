using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : MonoBehaviour
{
    [SerializeField] GameObject hpBar;

    [SerializeField] List<Transform> monsterList = new List<Transform>();
    [SerializeField] List<GameObject> hpBarList = new List<GameObject>();

    Vector3 hpBarOffset = new Vector3(0, -0.3f, 0);

    Camera cam = null;

    public void Setting(GameObject monster)
    {
        monsterList.Add(monster.transform);
        GameObject _hpBar = Instantiate(hpBar, monster.transform.position, Quaternion.identity, transform);
        _hpBar.GetComponent<Slider>().maxValue = monster.GetComponent<MonsterInfoBase>().Hp;
        _hpBar.GetComponent<Slider>().value = monster.GetComponent<MonsterInfoBase>().Hp;
        monster.GetComponent<MonsterActionBase>().hpBar = _hpBar.GetComponent<Slider>();
        hpBarList.Add(_hpBar);
    }

    void Awake()
    {
        //print("hpBar¼³Á¤");
        cam = Camera.main;

        /*GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        for(int i = 0; i < monsters.Length; i++)
        {
            monsterList.Add(monsters[i].transform);
            GameObject _hpBar = Instantiate(hpBar, monsters[i].transform.position, Quaternion.identity, transform);

            //int monsterHp = monsters[i].GetComponent<MonsterInfoBase>().Hp;
            //print(monsterHp);
            //_hpBar.GetComponent<Slider>().maxValue = monsterHp;
            //_hpBar.GetComponent<Slider>().value = monsterHp;
            monsters[i].GetComponent<MonsterActionBase>().hpBar = _hpBar.GetComponent<Slider>();
            hpBarList.Add(_hpBar);
        }*/
        //print("MonsterHpBar");
    }


    void Update()
    {
        for(int i = 0; i < monsterList.Count; i++)
        {
            hpBarList[i].transform.position = cam.WorldToScreenPoint(monsterList[i].position + hpBarOffset);
        }
    }
}
