using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.ParticleSystem;
using UnityEngine.AI;
using Unity.VisualScripting;

public class DaggerAction : PlayerActionBase
{
    [SerializeField] GameObject boss;
    [SerializeField] CapsuleCollider daggerCollider;
    [SerializeField] GameObject particle;
    [SerializeField] GameObject particle2;
    [SerializeField] GameObject particle3;

    private void OnEnable()
    {
        StartCoroutine(SkillARangeCheck());
    }

    protected override void Start()
    {
        base.Start();
        data = DataManager.instance.GetPlayerData(1002);
        daggerCollider.enabled = false;
        particle.SetActive(false);
        particle2.SetActive(false);
        particle3.SetActive(false);
        autoStandUpTime = data.auto_StandUp_Time;
    }

    protected override void PlayerAttackMotionCheck()
    {
        if (skilling || damaged)
        {
            // 만약 플레이어가 다운상태라면
            if (damaged && playerInfo.stun)
            {
                autoStandUpTime -= Time.deltaTime;

                if (autoStandUpTime <= 0f)
                {
                    autoStandUpTime = data.auto_StandUp_Time;
                    autoStandUp = true;
                }    

                // 기본공격대신 일어나는 애니메이션 취함
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
                //print("공격코루틴 초기화");
                StopCoroutine(attackCoroutine);
            }

            speed = 0;
            runMotion = false;
            animator.SetFloat("Move", (float)ePlayerState.Idle);
            attacking = true;
            animator.SetTrigger("Attack");
        }
    }

    IEnumerator NextAttackMotionCoolTime(float time)
    {
        yield return new WaitForSeconds(time);
        attacking = false;
        state = ePlayerState.Idle;
        //print("공격 초기화");
    }

    void AttackSound()
    {
        if(PlayerPrefs.GetInt("DaggerSwap") == 0)
            SoundManager.Instance.PlayPlayerSfx(PlayerSfx.DaggerWeapon);
        else
            SoundManager.Instance.PlayPlayerSfx(PlayerSfx.Hammer);
    }

    void SkillASound()
    {
        SoundManager.Instance.PlayPlayerSfx(PlayerSfx.DaggerSkillA);
    }

    void SkillBSound()
    {
        if (PlayerPrefs.GetInt("DaggerSwap") == 0)
            SoundManager.Instance.PlayPlayerSfx(PlayerSfx.DaggerSkillB);
        else
            SoundManager.Instance.PlayPlayerSfx(PlayerSfx.Hammer);
    }

    void SkillCSound()
    {
        if (PlayerPrefs.GetInt("DaggerSwap") == 0)
            SoundManager.Instance.PlayPlayerSfx(PlayerSfx.DaggerSkillC);
        else
            Invoke("HammerSound", 0.5f);
    }

    void HammerSound()
    {
        SoundManager.Instance.PlayPlayerSfx(PlayerSfx.Hammer);
    }

    // Attack1 이벤트 함수
    void CoolTime1()
    {
        attackCoroutine = StartCoroutine(NextAttackMotionCoolTime(0.4f));
    }

    // Attack2 이벤트 함수
    void CoolTime2()
    {
        attackCoroutine = StartCoroutine(NextAttackMotionCoolTime(0.35f));
    }

    // Attack3 이벤트 함수
    void CoolTime3()
    {
        attackCoroutine = StartCoroutine(NextAttackMotionCoolTime(0.85f));
    }

    protected override void AttackRange()
    {
        StartCoroutine(DaggerColliderOnOff());
        hitMonster = Physics.OverlapBox(transform.position + Vector3.up * 0.5f + transform.forward * 0.45f, new Vector3(0.9f, 0.5f, 0.5f) / 2, Quaternion.identity, 1 << 6);
        base.AttackRange();
    }

    IEnumerator DaggerColliderOnOff()
    {
        daggerCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        daggerCollider.enabled = false;
    }

    IEnumerator SkillARangeCheck()
    {
        Collider[] hitData;
        SkillButtonEvent go = GameObject.FindObjectOfType<SkillButtonEvent>();
        while (true)
        {
            yield return null;
            hitData = Physics.OverlapCapsule(transform.position + Vector3.up * 5f, transform.position - Vector3.up * 5f, 5f, 1 << 6);
            if (hitData.Length > 0)
            {
                go.SkillA_Onable();
                isSkillA = true;
            }
            else
            {
                go.SkillA_Disable();
                isSkillA = false;
            } 
        }
    }

    public bool isSkillA = false;
    bool isSkillC = false;

    protected override void SkillMotionCheck()
    {
        if (attacking || damaged)
            return;

        if (isSkillA && (Input.GetKeyDown(KeyCode.A) || skillA_ButtonClick) && !groundCheck.JumpingMotion && !skillA_Use && !skilling)
        {
            Invoke("SkillASound", 0.3f);

            FindObjectOfType<SkillButtonEvent>().SkillA_ButtonClick();
            skillA_ButtonClick = false;
            state = ePlayerState.Attack;
            skilling = true;
            StartCoroutine(SkillA_CoolDown_Time());
            speed = 0;
            runMotion = false;
            animator.SetFloat("Move", (float)ePlayerState.Idle);
            animator.SetTrigger("SkillA");
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
            Invoke("SkillCSound", 0.55f);

            FindObjectOfType<SkillButtonEvent>().SkillC_ButtonClick();
            isSkillC = true;
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

    [SerializeField] bool SkillA_Lock = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            SkillA_Lock = true;
            transform.DOKill();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
            SkillA_Lock = false;
    }

    // SkillA의 이벤트 함수
    void SkillA()
    {
        if (SkillA_Lock || isSkillC || playerTag.Tagging) { return; }
        StartCoroutine(SkillA_Delay());
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    IEnumerator SkillA_Delay()
    {
        yield return new WaitForSeconds(0.34f);

        if (transform.forward.x < 0f)
            transform.position = new Vector3(transform.position.x - 7f, transform.position.y + 2f, transform.position.z);
        else if (transform.forward.x > 0f)
            transform.position = new Vector3(transform.position.x + 7f, transform.position.y + 2f, transform.position.z);
    }

    // SkillB의 이벤트 함수
    protected override void SkillB_Range()
    {
        //print("스킬B!");
        hitMonster = Physics.OverlapBox(transform.position + Vector3.up * 0.5f + transform.forward * 0.45f, new Vector3(0.9f, 0.5f, 0.5f) / 2, Quaternion.identity, 1 << 6);
        //print("인식한 몬스터 수: " + hitMonster.Length);
        base.SkillB_Range();
    }

    // SkillC의 이벤트 함수
    protected override void SkillC_Range()
    {
        //print("스킬C!");
        hitMonster = Physics.OverlapBox(transform.position + Vector3.up * 0.5f + transform.forward * 0.45f, new Vector3(0.9f, 0.5f, 0.5f) / 2, Quaternion.identity, 1 << 6);
        //print("인식한 몬스터 수: " + hitMonster.Length);
        base.SkillC_Range();
    }

    void SkillCEnd()
    {
        //print("스킬 끝");
        skilling = false;
        state = ePlayerState.Idle;
    }


    public void TagSkill_Range(GameObject player)
    {
        PlayerInfoBase playerInfo;
        playerInfo = player.GetComponent<PlayerInfoBase>();
        //print("스킬B!");
        Collider[] hitMonster = Physics.OverlapBox(transform.position + Vector3.up * 0.5f + transform.forward * 0.1f, new Vector3(2.5f, 0.5f, 0.5f) / 2, Quaternion.identity, 1 << 6);
        //print("인식한 몬스터 수: " + hitMonster.Length);
        for (int i = 0; i < hitMonster.Length; i++)
        {
            hitMonster[i].GetComponent<MonsterActionBase>().Damaged(playerInfo);
        }
    }

    // 첫타 기본공격 이펙트
    void Attack_Effect()
    {
        if (transform.forward == Vector3.right)
        {
            particle.transform.position = new Vector3(transform.position.x + 1f, transform.position.y + 0.5f, 0f);
        }
        else
        {
            particle.transform.position = new Vector3(transform.position.x - 1f, transform.position.y + 0.5f, 0f);
        }
        particle.SetActive(true);
        particle.GetComponent<ParticleSystem>().Play();
    }

   

    void SkillA_Effect()
    {
        if (isSkillC || playerTag.Tagging)
        {
            isSkillC = false;
            return;
        }
        particle2.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        particle2.SetActive(true);
        particle2.GetComponent<ParticleSystem>().Play();
    }

    void SkillB_SkillC_Effect()
    {
        if (transform.forward == Vector3.right)
        {
            particle3.transform.position = new Vector3(transform.position.x + 1f, transform.position.y + 0.5f, 0f);
        }
        else
        {
            particle3.transform.position = new Vector3(transform.position.x - 1f, transform.position.y + 0.5f, 0f);
        }
        particle3.SetActive(true);
        particle3.GetComponent<ParticleSystem>().Play();
    }
}
