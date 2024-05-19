using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

public class WarriorAction : PlayerActionBase
{
    [SerializeField] GameObject particle;
    [SerializeField] GameObject particle2;
    [SerializeField] GameObject particle3;
    [SerializeField] GameObject particle4;
    [SerializeField] GameObject particle5;
    [SerializeField] GameObject particle6;
    [SerializeField] GameObject particle7;
    [SerializeField] GameObject particle8;

    [SerializeField] protected CapsuleCollider swordCollider;
    [SerializeField] protected GameObject sword;
    [SerializeField] protected GameObject spearFish;

    protected override void Awake()
    {
        base.Awake();
        swordCollider.enabled = false;

        Scene nowScene = SceneManager.GetSceneByName("Clear Stage");
        if (nowScene.name == "Clear Stage") return;

        particle.SetActive(false);
        particle2.SetActive(false);
        particle3.SetActive(false);
        particle4.SetActive(false);
        particle5.SetActive(false);
        particle6.SetActive(false);
        particle7.SetActive(false);
        particle8.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        data = DataManager.instance.GetPlayerData(1001);
    }

    void AttackSound()
    {
        SoundManager.Instance.PlayPlayerSfx(PlayerSfx.WarriorWeapon);
    }

    void SkillASound()
    {
        SoundManager.Instance.PlayPlayerSfx(PlayerSfx.WarriorSkillA);
    }

    void SkillBSound()
    {
        SoundManager.Instance.PlayPlayerSfx(PlayerSfx.WarriorSkillB);
    }

    void SkillCSound()
    {
        SoundManager.Instance.PlayPlayerSfx(PlayerSfx.WarriorSkillC);
    }

    protected override void SkillMotionCheck()
    {
        if (attacking || damaged)
            return;

        if ((Input.GetKeyDown(KeyCode.A) || skillA_ButtonClick) && !groundCheck.JumpingMotion && !skillA_Use && !skilling)
        {
            FindObjectOfType<SkillButtonEvent>().SkillA_ButtonClick();
            skillA_ButtonClick = false;
            state = ePlayerState.Attack;
            skilling = true;
            skillA_SizeUp = 1.5f;
            skillA_Use_BasicAttackRange = 0.25f;
            StartCoroutine(SkillA_CoolDown_Time());
            speed = 0;
            runMotion = false;
            animator.SetFloat("Move", (float)ePlayerState.Idle);
            animator.SetTrigger("SkillA");

            sword.transform.DOScale(data.skillA_Effect_SwordSizeUp, 0.5f);       // ��� ������ ��
            spearFish.transform.DOScale(data.skillA_Effect_SwordSizeUp, 0.5f);      // �и�ġ ������ ��                

            StartCoroutine(SwordSizeDown());
            StartCoroutine(SkillAEnd());
        }
        else if ((Input.GetKeyDown(KeyCode.S) || skillB_ButtonClick) && !groundCheck.JumpingMotion && !skillB_Use && !skilling)
        {
            FindObjectOfType<SkillButtonEvent>().SkillB_ButtonClick();
            skillB_ButtonClick = false;
            state = ePlayerState.Attack;
            skilling = true;
            StartCoroutine(SkillB_CoolDown_Time());
            speed = 0;
            runMotion = false;
            animator.SetFloat("Move", (float)ePlayerState.Idle);
            animator.SetTrigger("SkillB");
            StartCoroutine(SkillBEnd());
        }
        else if ((Input.GetKeyDown(KeyCode.D) || skillC_ButtonClick) && !groundCheck.JumpingMotion && !skillC_Use && !skilling)
        {
            FindObjectOfType<SkillButtonEvent>().SkillC_ButtonClick();
            skillC_ButtonClick = false;
            state = ePlayerState.Attack;
            skilling = true;
            StartCoroutine(SkillC_CoolDown_Time());
            speed = 0;
            runMotion = false;
            animator.SetFloat("Move", (float)ePlayerState.Idle);
            animator.SetTrigger("SkillC");
        }
    }

    public void Tag_SwordSizeDown()
    {
        sword.transform.DOScale(Vector3.one, 0.5f);
        spearFish.transform.DOScale(Vector3.one, 0.5f);
    }

    // SkillA �����ð�
    IEnumerator SwordSizeDown()
    {
        yield return new WaitForSeconds(data.skillA_Duration_Time);
        sword.transform.DOScale(Vector3.one, 0.5f);
        spearFish.transform.DOScale(Vector3.one, 0.5f);
        skillA_SizeUp = 1f;
        skillA_Use_BasicAttackRange = 0;
    }

    IEnumerator SwordColliderOnOff()
    {
        swordCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        swordCollider.enabled = false;
    }

    // ùŸ �⺻���� ����Ʈ
    void AttackEffect()
    {
        if (transform.forward == Vector3.right)
        {
            particle.transform.position = new Vector3(transform.position.x + 0.344f, transform.position.y + 0.621f, -0.057f);
            particle.transform.rotation = Quaternion.Euler(290f, -148f, 330f);
        }
        else
        {
            particle.transform.position = new Vector3(transform.position.x - 0.344f, transform.position.y + 0.621f, -0.057f);
            particle.transform.rotation = Quaternion.Euler(290f, -328f, 330f);
        }
        particle.SetActive(true);
        particle.transform.localScale = Vector3.one * 1.3f * skillA_SizeUp;
        particle.GetComponent<ParticleSystem>().Play();
    }

    // 2Ÿ �⺻���� ����Ʈ
    void AttackEffect2()
    {
        if (transform.forward == Vector3.right)
        {
            particle2.transform.position = new Vector3(transform.position.x + 0.344f, transform.position.y + 0.5f, -0.057f);
            particle2.transform.rotation = Quaternion.Euler(435f, 10f, 116f);
        }
        else
        {
            particle2.transform.position = new Vector3(transform.position.x - 0.344f, transform.position.y + 0.5f, -0.057f);
            particle2.transform.rotation = Quaternion.Euler(435f, -170f, 116f);
        }
        particle2.SetActive(true);
        particle2.transform.localScale = Vector3.one * 1.3f * skillA_SizeUp;
        particle2.GetComponent<ParticleSystem>().Play();
    }

    // 3Ÿ �⺻���� ����Ʈ
    void AttackEffect3()
    {
        if (transform.forward == Vector3.right)
        {
            particle3.transform.position = new Vector3(transform.position.x + 0.344f, transform.position.y + 0.7f, -0.057f);
            particle3.transform.rotation = Quaternion.Euler(0f, 0f, 100f);
        }
        else
        {
            particle3.transform.position = new Vector3(transform.position.x - 0.344f, transform.position.y + 0.7f, -0.057f);
            particle3.transform.rotation = Quaternion.Euler(0f, 180f, 100f);
        }
        particle3.SetActive(true);
        particle3.transform.localScale = Vector3.one * 1.3f * skillA_SizeUp;
        particle3.GetComponent<ParticleSystem>().Play();
    }

    // ùŸ �⺻���� ����Ʈ
    void TagSkill_Effect()
    {
        if (transform.forward == Vector3.right)
        {
            particle8.transform.position = new Vector3(transform.position.x + 1f, transform.position.y + 0.5f, 0f);
        }
        else
        {
            particle8.transform.position = new Vector3(transform.position.x - 1f, transform.position.y + 0.5f, 0f);
        }
        particle8.SetActive(true);
        particle8.GetComponent<ParticleSystem>().Play();
    }

    void SkillA_Effect()
    {
        if (transform.forward == Vector3.right)
        {
            particle4.transform.position = new Vector3(transform.position.x - 0.52f, transform.position.y + 1.469f, 0f);
        }
        else
        {
            particle4.transform.position = new Vector3(transform.position.x + 0.52f, transform.position.y + 1.469f, 0f);
        }
        particle4.SetActive(true);
        particle4.GetComponent<ParticleSystem>().Play();
    }

    void SkillB_Effect()
    {
        if (transform.forward == Vector3.right)
        {
            particle5.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, 0f);
            particle5.transform.rotation = Quaternion.Euler(90f, 0f, -45f);
        }
        else
        {
            particle5.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, 0f);
            particle5.transform.rotation = Quaternion.Euler(90f, 180f, -45f);
        }
        particle5.SetActive(false);
        particle5.SetActive(true);
        particle5.transform.localScale = Vector3.one * 1.3f * skillA_SizeUp;
    }

    void SkillB_Effect2()
    {
        if (transform.forward == Vector3.right)
        {
            particle6.transform.position = new Vector3(transform.position.x + 1.15f, transform.position.y + 0.5f, 0f);
            particle6.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            particle6.transform.position = new Vector3(transform.position.x - 1.15f, transform.position.y + 0.5f, 0f);
            particle6.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        particle6.SetActive(true);
        particle6.transform.localScale = Vector3.one;
        particle6.GetComponent<ParticleSystem>().Play();
    }

    void SkillC_Effect()
    {
        if (transform.forward == Vector3.right)
        {
            particle7.transform.position = new Vector3(transform.position.x, transform.position.y + 2.2f, 0f);
            particle7.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            particle7.transform.position = new Vector3(transform.position.x, transform.position.y + 2.2f, 0f);
            particle7.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        particle7.SetActive(true);
        particle7.transform.localScale = Vector3.one * 1.3f * skillA_SizeUp;
        particle7.GetComponent<ParticleSystem>().Play();
    }

    protected override void AttackRange()
    {
        StartCoroutine(SwordColliderOnOff());
        hitMonster = Physics.OverlapBox(transform.position + transform.forward * (0.7f + skillA_Use_BasicAttackRange) + Vector3.up * 0.5f, new Vector3(1f * skillA_SizeUp, 0.5f, 0.5f) / 2, Quaternion.identity, 1 << 6);
        base.AttackRange();
    }

    protected override void SkillB_Range()
    {
        //print("��ųB!");
        hitMonster = Physics.OverlapBox(transform.position + Vector3.up * 0.5f + transform.forward * 0.1f, new Vector3(2.5f * skillA_SizeUp, 0.5f, 0.5f) / 2, Quaternion.identity, 1 << 6);
        //print("�ν��� ���� ��: " + hitMonster.Length);
        base.SkillB_Range();
    }

    // SkillC�� �̺�Ʈ �Լ�
    protected override void SkillC_Range()
    {
        //print("��ųC!");
        hitMonster = Physics.OverlapBox(transform.position + Vector3.up * 2.2f, new Vector3(2.6f * skillA_SizeUp, 2.6f * skillA_SizeUp, 0.5f) / 2, Quaternion.identity, 1 << 6);
        //print("�ν��� ���� ��: " + hitMonster.Length);
        base.SkillC_Range();
    }

    // �⺻����
    protected override void PlayerAttackMotionCheck()
    {
        if (skilling || damaged)
        {
            // ���� �÷��̾ �ٿ���¶��
            if (damaged && playerInfo.stun)
            {
                autoStandUpTime -= Time.deltaTime;

                if (autoStandUpTime <= 0f)
                {
                    autoStandUpTime = data.auto_StandUp_Time;
                    autoStandUp = true;
                }

                // �⺻���ݴ�� �Ͼ�� �ִϸ��̼� ����
                if ((Input.GetKeyDown(KeyCode.Z) || autoStandUp) && !groundCheck.JumpingMotion)
                {
                    autoStandUpTime = data.auto_StandUp_Time;
                    autoStandUp = false;
                    animator.SetTrigger("StandUp");
                    playerInfo.stun = false;
                    return;
                }
            }

            return;
        }

        if ((Input.GetKeyDown(KeyCode.Z) || attackButtonClick) && !groundCheck.JumpingMotion)
        {
            attackButtonClick = false;
            state = ePlayerState.Attack;

            if (attackCoroutine != null)
            {
                //print("�����ڷ�ƾ �ʱ�ȭ");
                StopCoroutine(attackCoroutine);
            }

            speed = 0;
            runMotion = false;
            animator.SetFloat("Move", (float)ePlayerState.Idle);
            attacking = true;
            animator.SetTrigger("Attack");
            attackCoroutine = StartCoroutine(NextAttackMotionCoolTime());
        }
    }



    protected override IEnumerator NextAttackMotionCoolTime()
    {
        yield return new WaitForSeconds(1f);
        attacking = false;
        state = ePlayerState.Idle;
        //print("���� �ʱ�ȭ");
    }

    // Attack2 ��ǿ� �̺�Ʈ �Լ��� ��ϵ�����
    void Attack2MoveFront()
    {
        StartCoroutine(Attack2MoveFrontCo());
    }

    // �⺻���� 2Ÿ°�� ��¦ �����ϴ� �ڷ�ƾ
    IEnumerator Attack2MoveFrontCo()
    {
        float time = data.movement_Time_During_Attack;
        while (time > 0f)
        {
            rig.MovePosition(transform.position + data.movement_Distance_During_Attack * Time.fixedDeltaTime * transform.forward);
            time -= Time.deltaTime;
            yield return null;
        }
    }

    public void TagSkill_Range(GameObject player)
    {
        PlayerInfoBase playerInfo;
        playerInfo = player.GetComponent<PlayerInfoBase>();
        //print("��ųB!");
        Collider[] hitMonster = Physics.OverlapBox(transform.position + Vector3.up * 0.5f + transform.forward * 0.1f, new Vector3(2.5f, 0.5f, 0.5f) / 2, Quaternion.identity, 1 << 6);
        //print("�ν��� ���� ��: " + hitMonster.Length);
        for (int i = 0; i < hitMonster.Length; i++)
        {
            hitMonster[i].GetComponent<MonsterActionBase>().Damaged(playerInfo);
        }
    }
}
