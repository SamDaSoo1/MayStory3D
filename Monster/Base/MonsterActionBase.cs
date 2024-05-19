using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MonsterActionBase : MonoBehaviour
{
    [SerializeField] GameObject hit_Particle;
    protected int key;
    public eMonsterState monsterState;
    [SerializeField] protected GameObject damageText;
    [SerializeField] protected List<PlayerInfoBase> playerInfo;
    [SerializeField] protected List<Transform> player;
    protected MonsterNavBase monsterNav;
    [SerializeField] protected MonsterInfoBase monsterInfo;
    public Animator animator;
    protected Rigidbody rig;
    public Slider hpBar;
    protected float attackCoolTime;                
    protected float knockBackDistance;                
    protected int attackCount;
    [SerializeField] MonsterHpBar monsterHpBar;
    int sorting = 0;

    public void PlayerSetting(GameObject[] list)
    {
        foreach (GameObject item in list)
        {
            playerInfo.Add(item.GetComponent<PlayerInfoBase>());
            player.Add(item.transform);
        }
    }

    protected virtual void Awake()
    {
        monsterState = eMonsterState.Idle;
        monsterNav = GetComponent<MonsterNavBase>();
        monsterInfo = GetComponent<MonsterInfoBase>();
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        attackCoolTime = 0f;
        attackCount = 0;
    }

    void Start()
    {
        hit_Particle.SetActive(false);
        damageText = Resources.Load<GameObject>("DamageText2");
        monsterHpBar = GameObject.FindObjectOfType<MonsterHpBar>();
        knockBackDistance = DataManager.instance.GetMonsterData(key).knockBackDistance;
        Scene nowScene = SceneManager.GetSceneByName("Stage");
        if (nowScene.name == "Stage")
            monsterHpBar.Setting(gameObject);
        hpBar.maxValue = DataManager.instance.GetMonsterData(key).hp;
        hpBar.value = hpBar.maxValue;
    }

    protected virtual void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            return;

        if (playerInfo[0].gameObject.activeSelf == true)
            if (playerInfo[0].GetPlayerState() == ePlayerState.Dead)
                return;

        if (playerInfo[1].gameObject.activeSelf == true)
            if (playerInfo[1].GetPlayerState() == ePlayerState.Dead)
                return;

        AttackCoolTimeCheck();
    }

    public virtual void Damaged(PlayerInfoBase _player, float AdditionalDamage = 1)
    {
        hit_Particle.SetActive(false);
        if(gameObject.name == "Flower Dryad")
            hit_Particle.transform.position = transform.position + Vector3.up * 1.5f;
        else
            hit_Particle.transform.position = transform.position + Vector3.up * 0.5f;
        hit_Particle.SetActive(true);
        animator.StopPlayback();

        if(gameObject.name != "Flower Dryad")
            animator.Play("Damage", -1, 0);

        monsterState = eMonsterState.Angry;
        monsterNav.PlayerDetection(_player.transform);
        //rig.MovePosition(transform.position + player.forward * knockBackDistance);
        transform.position += _player.transform.forward * knockBackDistance;

        int ranNum = Random.Range(-29, 31);
        monsterInfo.Hp -= (int)Mathf.Round(_player.offense_power * AdditionalDamage) + ranNum;
        Instantiate(damageText).GetComponent<DamagedText>().SetUp((int)Mathf.Round(_player.offense_power * AdditionalDamage) + ranNum, transform, ++sorting);

        if (hpBar != null)
        {
            hpBar.value -= (int)Mathf.Round(_player.offense_power * AdditionalDamage) + ranNum;

            if (hpBar.value <= 0f)
                StartCoroutine(HpBarDisappear());
        } 
        else if (hpBar == null && gameObject.name == "Flower Dryad")
        {
            GetComponent<FlowerDryadInfo>().Damage((int)Mathf.Round(_player.offense_power * AdditionalDamage) + ranNum);
        }

        //print("몬스터 HP: " + monsterInfo.Hp);
    }

    IEnumerator HpBarDisappear()
    {
        yield return new WaitForSeconds(5f);
        hpBar.gameObject.SetActive(false);
    }

    // Attack의 이벤트 함수
    /// <summary>
    /// Collider[] hitData = Physics.OverlapBox();로 검출하고
    /// for문을 이용하여 hitData[i].GetComponent<PlayerAction>().Damaged(monsterInfo); 해당 객체에 대미지주기
    public virtual void AttackRange() { }

    public bool isAttack = false;

    void AttackCoolTimeCheck()
    {
        if (attackCoolTime > 0f && isAttack == false)
        {
            attackCoolTime -= Time.deltaTime;
        } 

        if (monsterState == eMonsterState.Angry && attackCoolTime <= 0f && !animator.GetBool("Move"))
        {
            isAttack = true;
            attackCoolTime = DataManager.instance.GetMonsterData(key).attackCoolTime;

            // 맨 처음만 피격 애니메이션 때문에 모션딜레이 생겨서 1초 더함
            /*if (attackCount == 0)
                attackCoolTime = DataManager.instance.GetMonsterData(key).attackCoolTime + 1;
            else
                attackCoolTime = DataManager.instance.GetMonsterData(key).attackCoolTime;*/

            animator.SetTrigger("Attack");
            if (gameObject.name != "Flower Dryad")
                isAttack = false;
            //attackCount++;
        }
    }
}
