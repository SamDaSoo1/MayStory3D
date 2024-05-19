using UnityEngine;
using DG.Tweening;
using System.Collections;

public class FlowerDryadAction: MonsterActionBase
{
    [SerializeField] Summons summons;
    [SerializeField] ThunderBoltController thunderBolt;
    [SerializeField] BossSkillEffect bossSkillEffect;

    public bool invincibility;
    int skillCount;
    float skillCoolTime;

    public bool skillChoice = false;

    protected override void Awake()
    {
        key = 201;
        base.Awake();
        invincibility = false;
        skillCount = 0;
        skillCoolTime = 11f;
        attackCoolTime = 3f;
    }

    private void Start()
    {
        damageText = Resources.Load<GameObject>("DamageText3");
        summons = GameObject.FindObjectOfType<Summons>();
        thunderBolt = GameObject.FindObjectOfType<ThunderBoltController>();
        StartCoroutine(Detection_Area());
    }

    IEnumerator Detection_Area()
    {
        float distance = 0f;
        Transform curPlayer = null;
        while(true)
        {
            yield return null;
            foreach(Transform t in player)
            {
                if (t.gameObject.activeSelf)
                {
                    curPlayer = t;
                    break;
                }   
            }
            distance = Vector3.Distance(curPlayer.position, transform.position);

            if (distance < 6f)
            {
                GameObject.FindObjectOfType<BossScene_CameraMove>().Cam2On();
                yield break;
            }
        }
    }

    public void Chase()
    {
        GetComponent<MonsterNavBase>().monsterState = eMonsterState.Angry;
        monsterState = eMonsterState.Angry;
    }

    protected override void Update()
    {
        //print($"{attackCoolTime}, {skillCoolTime}");
        if (FindObjectOfType<BossScene_CameraMove>().BattleOn == false) return;
        base.Update();
        SkillCoolTimeCheck();
    }

    void SkillCoolTimeCheck()
    {
        if (skillCoolTime > 0f)
        {
            skillCoolTime -= Time.deltaTime;
            //print(skillCoolTime);
        }

        if (monsterState == eMonsterState.Angry && skillCoolTime <= 0f && !animator.GetBool("Move") && !skillChoice)
        {
            skillChoice = true;
            animator.SetTrigger("Skill 2");  
        }
    }

    public override void Damaged(PlayerInfoBase _player, float AdditionalDamage = 1)
    {
        if(invincibility) { return; }
        base.Damaged(_player, AdditionalDamage);
        SoundManager.Instance.PlayBossSfx(BossSfx.BossDamage);
    }

    void AttackEffect()
    {
        if (skillChoice == true)
            skillChoice = false;
        SoundManager.Instance.PlayBossSfx(BossSfx.BossAttack);
        bossSkillEffect.Shoot(transform);
    }

    void Skill2Effect()
    {
        SoundManager.Instance.PlayBossSfx(BossSfx.BossSkill_Start);
        bossSkillEffect.Scatter(transform);
    }

    // Attack의 이벤트 함수
    public override void AttackRange()  
    {
        Collider[] hitData = Physics.OverlapBox(transform.position + Vector3.up * 0.5f + transform.forward * 2f, new Vector3(4f, 1, 1) / 2, Quaternion.identity, 1 << 7);
        //print(hitData.Length);
        for(int i = 0; i < hitData.Length; i++)
        {
            hitData[i].GetComponent<PlayerActionBase>()?.Damaged(monsterInfo);
        }
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.up * 0.5f, new Vector3(8f, 1, 1));
    }*/

    // Skill의 이벤트 함수
    void SkillRange()
    {
        Collider[] hitData = Physics.OverlapBox(transform.position + Vector3.up * 0.5f, new Vector3(8f, 1, 1) / 2, Quaternion.identity, 1 << 7);
        //print(hitData.Length);
        for (int i = 0; i < hitData.Length; i++)
        {
            hitData[i].GetComponent<PlayerActionBase>()?.Damaged(monsterInfo);
        }
    }

    // Skill 모션에 이벤트 함수로 등록되있음
    // Skill 모션 시작 시 무적On
    void InvincibilityOn()
    {
        //print("무적온");
        invincibility = true;
        skillChoice = false;

        if (skillCount == 0)
        {
            // 맨 처음만 피격 애니메이션 때문에 모션딜레이 생겨서 쿨타임 11초로 설정함
            skillCoolTime = 11f;
        }
        else
        {
            skillCoolTime = DataManager.instance.GetMonsterData(key).skillCoolTime;
        } 

        skillCount++;
    }

    // Skill 모션에 이벤트 함수로 등록되있음
    // Skill 모션 끝날 때 무적Off
    void InvincibilityOff()
    {
        //print("무적오프");
        invincibility = false;
    }

    void Skill1()
    {
        summons.Create(transform);
    }

    void Skill2()
    {
        thunderBolt.Strike(transform);
    }
}
