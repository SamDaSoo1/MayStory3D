using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class FlowerDryadInfo : MonsterInfoBase
{
    [SerializeField] GameObject particle;
    [SerializeField] GameObject bossHp;
    [SerializeField] List<Image> hpBar;
    [SerializeField] TextMeshProUGUI x4;
    [SerializeField] ClearText clearText;
    BossScene_CameraMove bc;
    int health = 4000;

    protected override void Start()
    {
        key = 201;                       // 나중에 CSV 참고해서 FlowerDryad의 키 값으로 설정하기.
        base.Start();
        bossHp = GameObject.Find("BossHp");
        hpBar = new List<Image>();
        hpBar.Add(GameObject.Find("Hp 1").GetComponent<Image>());
        hpBar.Add(GameObject.Find("Hp 2").GetComponent<Image>());
        hpBar.Add(GameObject.Find("Hp 3").GetComponent<Image>());
        hpBar.Add(GameObject.Find("Hp 4").GetComponent<Image>());
        bossHp.SetActive(false);
        particle.SetActive(false);

        clearText = GameObject.FindObjectOfType<ClearText>();
        bc = GameObject.FindAnyObjectByType<BossScene_CameraMove>();
    }

    public void Damage(int damage)
    {
        if(bossHp.activeSelf == false) { bossHp.SetActive(true); }
        
        health -= damage;
        if (health < 0)
        {
            GameObject.FindObjectOfType<PlayerActionBase>().isClear = true;
            bc.BattleOn = false;
            bc.BattleEnd();
            hpBar[3].fillAmount = 0;
            health = 0;
            clearText.CoStart();
            StartCoroutine(HpBarOff());
            StartCoroutine(SlowMotion());
        }
        else if(health < 1000)
        {
            hpBar[2].fillAmount = 0;
            x4.enabled = false;
            hpBar[3].fillAmount = health / 1000f;
        }
        else if (health < 2000)
        {
            hpBar[1].fillAmount = 0;
            x4.text = "x 1";
            hpBar[2].fillAmount = (health - 1000) / 1000f;
        }
        else if (health < 3000)
        {
            hpBar[0].fillAmount = 0;
            x4.text = "x 2";
            hpBar[1].fillAmount = (health - 2000) / 1000f;
        }
        else if (health < 4000)
        {
            if (x4.enabled == false) { x4.enabled = true; x4.text = "x 3"; }
            hpBar[0].fillAmount = (health - 3000) / 1000f;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
            clearText.CoStart();
    }

    IEnumerator HpBarOff()
    {
        yield return new WaitForSeconds(1);
        particle.transform.position = transform.position;
        particle.SetActive(true);
        yield return new WaitForSeconds(4);
        bossHp.SetActive(false);
    }

    IEnumerator SlowMotion()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1f;
    }
}
