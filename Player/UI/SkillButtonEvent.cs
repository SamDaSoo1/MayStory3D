using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonEvent : MonoBehaviour
{
    [SerializeField] List<PlayerActionBase> playerSkill;
    [SerializeField] Image skillACoolTimeImg;
    [SerializeField] Image skillBCoolTimeImg;
    [SerializeField] Image skillCCoolTimeImg;
    [SerializeField] TextMeshProUGUI skillACoolTimeText;
    [SerializeField] TextMeshProUGUI skillBCoolTimeText;
    [SerializeField] TextMeshProUGUI skillCCoolTimeText;
    Portal portal;

    private void Awake()
    {
        playerSkill = new List<PlayerActionBase>
        {
            GameObject.Find("Player").transform.GetChild(0).GetComponent<WarriorAction>(),
            GameObject.Find("Player").transform.GetChild(1).GetComponent<DaggerAction>()
        };
        skillACoolTimeImg = transform.GetChild(5).transform.GetChild(0).GetComponent<Image>();
        skillBCoolTimeImg = transform.GetChild(6).transform.GetChild(0).GetComponent<Image>();
        skillCCoolTimeImg = transform.GetChild(7).transform.GetChild(0).GetComponent<Image>();
        skillACoolTimeImg.enabled = false;
        skillBCoolTimeImg.enabled = false;
        skillCCoolTimeImg.enabled = false;
        skillACoolTimeText = transform.GetChild(5).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        skillBCoolTimeText = transform.GetChild(6).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        skillCCoolTimeText = transform.GetChild(7).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        skillACoolTimeText.enabled = false; 
        skillBCoolTimeText.enabled = false;
        skillCCoolTimeText.enabled = false;
    }

    private void Start()
    {
        portal = GameObject.FindObjectOfType<Portal>();
    }

    public void AttackButtonClick()
    {
        if (playerSkill[0].gameObject.activeSelf == true)
            playerSkill[0].attackButtonClick = true;
        else
            playerSkill[1].attackButtonClick = true;
    }

    public void SkillA_ButtonClick()
    {
        if (playerSkill[0].gameObject.activeSelf == true)
        {
            playerSkill[0].skillA_ButtonClick = true;
            Button btn = transform.GetChild(5).GetComponentInParent<Button>();
            btn.interactable = false;
            skillACoolTimeImg.enabled = true;
            skillACoolTimeText.enabled = true;
            StartCoroutine(Skill_CoolTime(btn, skillACoolTimeImg, playerSkill[0].data.skillA_CoolDown_Time, skillACoolTimeText));
        }
        else
        {
            playerSkill[1].skillA_ButtonClick = true;
            Button btn = transform.GetChild(5).GetComponentInParent<Button>();
            btn.interactable = false;
            skillACoolTimeImg.enabled = true;
            skillACoolTimeText.enabled = true;
            StartCoroutine(Skill_CoolTime(btn, skillACoolTimeImg, playerSkill[1].data.skillA_CoolDown_Time, skillACoolTimeText));
        }
    }

    public void SkillB_ButtonClick()
    {
        if (playerSkill[0].gameObject.activeSelf == true)
        {
            playerSkill[0].skillB_ButtonClick = true;
            Button btn = transform.GetChild(6).GetComponentInParent<Button>();
            btn.interactable = false;
            skillBCoolTimeImg.enabled = true;
            skillBCoolTimeText.enabled = true;
            StartCoroutine(Skill_CoolTime(btn, skillBCoolTimeImg, playerSkill[0].data.skillB_CoolDown_Time, skillBCoolTimeText));
        }
        else
        {
            playerSkill[1].skillB_ButtonClick = true;
            Button btn = transform.GetChild(6).GetComponentInParent<Button>();
            btn.interactable = false;
            skillBCoolTimeImg.enabled = true;
            skillBCoolTimeText.enabled = true;
            StartCoroutine(Skill_CoolTime(btn, skillBCoolTimeImg, playerSkill[1].data.skillB_CoolDown_Time, skillBCoolTimeText));
        }
            
    }

    public void SkillC_ButtonClick()
    {
        if (playerSkill[0].gameObject.activeSelf == true)
        {
            playerSkill[0].skillC_ButtonClick = true;
            Button btn = transform.GetChild(7).GetComponentInParent<Button>();
            btn.interactable = false;
            skillCCoolTimeImg.enabled = true;
            skillCCoolTimeText.enabled = true;
            StartCoroutine(Skill_CoolTime(btn, skillCCoolTimeImg, playerSkill[0].data.skillC_CoolDown_Time, skillCCoolTimeText));
        }
        else
        {
            playerSkill[1].skillC_ButtonClick = true;
            Button btn = transform.GetChild(7).GetComponentInParent<Button>();
            btn.interactable = false;
            skillCCoolTimeImg.enabled = true;
            skillCCoolTimeText.enabled = true;
            StartCoroutine(Skill_CoolTime(btn, skillCCoolTimeImg, playerSkill[1].data.skillC_CoolDown_Time, skillCCoolTimeText));
        }  
    }

    public void SkillA_Disable()
    {
        if (!gameObject.activeSelf) return;
        transform.GetChild(5).GetComponentInParent<Button>().interactable = false;
    }

    public void SkillA_Onable()
    {
        Button btn = transform.GetChild(5).GetComponentInParent<Button>();
        if (btn == null) return;
        btn.interactable = true;
    }

    public void Tag_Skill_CoolTime(GameObject player)
    {
        StopAllCoroutines();
        skillACoolTimeImg.fillAmount = 0f;
        skillBCoolTimeImg.fillAmount = 0f;
        skillCCoolTimeImg.fillAmount = 0f;
        skillACoolTimeText.text = 0.ToString();
        skillBCoolTimeText.text = 0.ToString();
        skillCCoolTimeText.text = 0.ToString();
        skillACoolTimeImg.enabled = false;
        skillBCoolTimeImg.enabled = false;
        skillCCoolTimeImg.enabled = false;
        skillACoolTimeText.enabled = false;
        skillBCoolTimeText.enabled = false;
        skillCCoolTimeText.enabled = false;
        transform.GetChild(5).GetComponentInParent<Button>().interactable = true;
        transform.GetChild(6).GetComponentInParent<Button>().interactable = true;
        transform.GetChild(7).GetComponentInParent<Button>().interactable = true;
    }

    IEnumerator Skill_CoolTime(Button btn, Image coolTimeImg, float coolTime, TextMeshProUGUI txt)
    {
        float time = coolTime;
        txt.text = time.ToString();
        while (time > 0f)
        {
            time -= Time.deltaTime;
            txt.text = Mathf.Round(time).ToString();
            coolTimeImg.fillAmount = time / coolTime;
            yield return null;
        }
        coolTimeImg.fillAmount = 0f;
        txt.text = 0.ToString();
        coolTimeImg.enabled = false;
        txt.enabled = false;
        btn.interactable = true;
    }

    public void LeftClickDown()
    {
        if (playerSkill[0].gameObject.activeSelf == true)
        {
            playerSkill[0].MyGetAxisRaw = -1f;
            playerSkill[0].leftRunMotionCount++;
        }
        else
        {
            playerSkill[1].MyGetAxisRaw = -1f;
            playerSkill[1].leftRunMotionCount++;
        }
    }

    public void LeftClickUp()
    {
        if (playerSkill[0].gameObject.activeSelf == true)
        {
            playerSkill[0].MyGetAxisRaw = 0f;
        }
        else
        {
            playerSkill[1].MyGetAxisRaw = 0f;
        }
    }

    public void RightClickDown()
    {
        if (playerSkill[0].gameObject.activeSelf == true)
        {
            playerSkill[0].MyGetAxisRaw = 1f;
            playerSkill[0].rightRunMotionCount++;
        }
        else
        {
            playerSkill[1].MyGetAxisRaw = 1f;
            playerSkill[1].rightRunMotionCount++;
        }
    }

    public void RightClickUp()
    {
        if (playerSkill[0].gameObject.activeSelf == true)
        {
            playerSkill[0].MyGetAxisRaw = 0f;
        }
        else
        {
            playerSkill[1].MyGetAxisRaw = 0f;
        }
    }

    public void DownClickDown()
    {
        if (portal == null)
            return;
        portal.DownArrowClick = true;
    }

    public void UpClickDown()
    {
        if (playerSkill[0].gameObject.activeSelf == true)
        {
            playerSkill[0].GetComponentInChildren<GroundCheck>().UpClick = true;
        }
        else
        {
            playerSkill[1].GetComponentInChildren<GroundCheck>().UpClick = true;
        }
    }
}
