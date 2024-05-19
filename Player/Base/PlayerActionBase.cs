using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ePlayerState { Idle, Walk, Run, Jump, Attack, Alive, Dead };

public class PlayerActionBase : MonoBehaviour
{
    [SerializeField] GameObject damageText;
    [SerializeField] Image damagedRed;
    [SerializeField] Image blocking;
    [SerializeField] protected GroundCheck groundCheck;
    [SerializeField] TextMeshProUGUI hpText;
    protected ePlayerState state;
    [SerializeField] protected PlayerInfoBase playerInfo;
    protected Animator animator;
    protected Rigidbody rig;
    protected PlayerTag playerTag;
    protected Coroutine attackCoroutine = null;

    protected Vector3 movement = Vector3.zero;

    protected float changeRunModeCoolTime = 0.5f;
    protected float changeDirectionOfRunningCoolTime = 0.1f;
    protected float changeDirectionOfRunningCoolTimeCheck = 0.1f;
    protected float xPos = 0f;
    protected float speed = 0;
    protected float skillA_SizeUp = 1;
    protected float skillA_Use_BasicAttackRange = 0;
    protected float autoStandUpTime = 2f;

    public int rightRunMotionCount = 0;
    public int leftRunMotionCount = 0;

    public bool attacking = false;
    public bool skilling = false;
    public bool damaged = false;
    public bool skillA_Use = false;
    public bool skillB_Use = false;
    public bool skillC_Use = false;

    protected bool runMotion = false;
    protected bool autoStandUp = false;

    public bool attackButtonClick = false;
    public bool skillA_ButtonClick = false;
    public bool skillB_ButtonClick = false;
    public bool skillC_ButtonClick = false;

    public float MyGetAxisRaw {  get; set; }

    public PlayerData data;
    int sorting = 0;

    public bool isClear = false;

    protected virtual void Awake()
    {
        state = ePlayerState.Idle;
        groundCheck = GetComponentInChildren<GroundCheck>();
        playerInfo = GetComponent<PlayerInfoBase>();
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        damageText = Resources.Load<GameObject>("DamageText");

        if (gameObject.name == "Warrior")
            damagedRed = GameObject.Find("Damaged Blue").GetComponent<Image>();
        else if (gameObject.name == "Dagger")
            damagedRed = GameObject.Find("Damaged Grape").GetComponent<Image>();

        playerTag = GameObject.FindObjectOfType<PlayerTag>();
        hpText = GameObject.Find("Canvas 1").transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            return;

        if (animator.GetFloat("Move") == (float)ePlayerState.Walk || animator.GetFloat("Move") == (float)ePlayerState.Run)
            Moving(xPos);
    }

    public void DontMove()
    {
        xPos = 0;
        runMotion = false;
        animator.SetFloat("Move", (float)ePlayerState.Idle);
        speed = 0;
    }

    void Update()
    {
        if (blocking.enabled || (FindObjectOfType<BossScene_CameraMove>() != null && FindObjectOfType<BossScene_CameraMove>().isCamTransition))
            return;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die") || playerTag.Tagging)
            return;

        SkillMotionCheck();

        PlayerAttackMotionCheck();

        if (attacking || skilling || damaged)
            return;

        if(Input.GetAxisRaw("Horizontal") == 0f)
            xPos = MyGetAxisRaw;
        else
            xPos = Input.GetAxisRaw("Horizontal");

        ChangeDirectionOfRunning(xPos, ref runMotion);

        PlayerState(xPos, runMotion);

        PlayerDirection(xPos);

        PlayerRunMotionCheck(xPos);
    }

    public void Damaged(MonsterInfoBase _monster)
    {
        if (skilling || playerInfo.stun || playerInfo.state == ePlayerState.Dead || playerTag.Tagging || isClear)
            return;

        SoundManager.Instance.PlayPlayerSfx(PlayerSfx.PlayerDamage);

        InitState();

        rig.MovePosition(transform.position + _monster.GetComponent<Transform>().forward * data.knockBackDistance);

        if (_monster.GetComponent<Transform>().forward != transform.forward)
        {
            // ���Ϳ� ���ֺ��� �ִٸ� ���鿡�� ���� �ǰ� �ִϸ��̼� ����
            animator.Play("DamagedFront", -1, 0);
        }
        else
        {
            // ���Ϳ��� ���� �����ٸ� �ĸ鿡�� ���� �ǰ� �ִϸ��̼� ����
            animator.Play("DamagedBack", -1, 0);
        }

        playerInfo.LastMonsterhit(_monster.GetComponent<Transform>());
        int ranNum = Random.Range(-5, 6);
        if(gameObject.name == "Flower Dyrad")
            ranNum = Random.Range(-20, 21);
        playerInfo.Hp -= _monster.AttackPower + ranNum;
        if(gameObject.name == "Warrior")
            playerTag._hp[0] -= _monster.AttackPower + ranNum;
        else
            playerTag._hp[1] -= _monster.AttackPower + ranNum;

        Instantiate(damageText).GetComponent<DamagedText>().SetUp(_monster.AttackPower + ranNum, transform, sorting++);

        hpText.text = playerInfo.Hp + "/" + playerInfo.maxHp;
        StartCoroutine(DamagedRedProfile());
        //print("�÷��̾� HP: " + playerInfo.Hp);
    }

    IEnumerator DamagedRedProfile()
    {
        damagedRed.enabled = true;
        yield return new WaitForSeconds(0.1f);
        damagedRed.enabled = false;
        yield return new WaitForSeconds(0.1f);
        damagedRed.enabled = true;
        yield return new WaitForSeconds(0.1f);
        damagedRed.enabled = false;
    }

    public void Damaged(int damage)
    {
        if (skilling || playerInfo.stun || GetComponent<PlayerInfoBase>().state == ePlayerState.Dead || isClear)
            return;

        InitState();

        animator.Play("DamagedFront", -1, 0);

        playerInfo.Hp -= damage;
        Instantiate(damageText).GetComponent<DamagedText>().SetUp(damage, transform, sorting++);

        if (gameObject.name == "Warrior")
            playerTag._hp[0] -= damage;
        else
            playerTag._hp[1] -= damage;
        hpText.text = playerInfo.Hp + "/" + playerInfo.maxHp;
        //print("�÷��̾� HP: " + playerInfo.Hp);

    }

    // ��ų ���
    protected virtual void SkillMotionCheck() { }

    // SkillA ��Ÿ��
    protected IEnumerator SkillA_CoolDown_Time()
    {
        skillA_Use = true;
        yield return new WaitForSeconds(data.skillA_CoolDown_Time);
        skillA_Use = false;
    }

    // SkillB ��Ÿ��
    protected IEnumerator SkillB_CoolDown_Time()
    {
        skillB_Use = true;
        yield return new WaitForSeconds(data.skillB_CoolDown_Time);
        skillB_Use = false;
    }

    // SkillC ��Ÿ��
    protected IEnumerator SkillC_CoolDown_Time()
    {
        skillC_Use = true;
        yield return new WaitForSeconds(data.skillC_CoolDown_Time);
        skillC_Use = false;
    }

    // SkillA �ĵ�
    protected IEnumerator SkillAEnd()
    {
        yield return new WaitForSeconds(data.skillA_DelayTime_AfterAction);
        skilling = false;
        state = ePlayerState.Idle;
    }

    // SkillB �ĵ�
    protected IEnumerator SkillBEnd()
    {
        yield return new WaitForSeconds(data.skillB_DelayTime_AfterAction);
        skilling = false;
        state = ePlayerState.Idle;
    }

    // SkillC ��ǿ� �̺�Ʈ �Լ��� ��ϵ�����
    // SkillC �ĵ�
    void SkillCEnd()
    {
        skilling = false;
        state = ePlayerState.Idle;
    }

    // �⺻����
    protected virtual void PlayerAttackMotionCheck() { }

    // �⺻���� �ĵ�
    protected virtual IEnumerator NextAttackMotionCoolTime() { yield return null; }

    protected Collider[] hitMonster;

    // Attack�� �̺�Ʈ �Լ�
    protected virtual void AttackRange()
    {
        for (int i = 0; i < hitMonster.Length; i++)
        {
            hitMonster[i].GetComponent<MonsterActionBase>().Damaged(playerInfo, data.basic_Attack);
        }
    }

    // SkillB�� �̺�Ʈ �Լ�
    protected virtual void SkillB_Range()
    {
        for (int i = 0; i < hitMonster.Length; i++)
        {
            hitMonster[i].GetComponent<MonsterActionBase>().Damaged(playerInfo, data.skillB_Damage);
        }
    }

    // SkillC�� �̺�Ʈ �Լ�
    protected virtual void SkillC_Range()
    {
        for (int i = 0; i < hitMonster.Length; i++)
        {
            hitMonster[i].GetComponent<MonsterActionBase>().Damaged(playerInfo, data.skillC_Damage);
        }
    }

    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.up * 2.2f, new Vector3(2.6f * skillA_SizeUp, 2.6f * skillA_SizeUp, 0.5f));
    }*/

    

    // Attack3 ��ǿ� �̺�Ʈ �Լ��� ��ϵ�����
    void AttackMotionEnd()
    {
        animator.ResetTrigger("Attack");
        attacking = false;
        state = ePlayerState.Idle;
    }

    // �÷��̾� ������ ���� �Լ�
    void Moving(float xPos)
    {
        movement.Set(xPos, 0, 0);
        movement = movement.normalized * speed * Time.deltaTime;
        rig.MovePosition(transform.position + movement);
        //rig.AddForce(new Vector3(xPos, 0, 0) * 200);
        //rig.velocity = movement;
    }

    // �޸��� ���� ���� ��ȯ�� �� 0.1�ʾȿ� �ٲٸ� �޸��� ����
    void ChangeDirectionOfRunning(float xPos, ref bool runMotion)
    {
        if (xPos == 0 && runMotion)
        {
            changeDirectionOfRunningCoolTimeCheck -= Time.deltaTime;
            //print("���� ��ȯ ��Ÿ��: " + ChangeDirectionOfRunningCoolTime);
            if (changeDirectionOfRunningCoolTimeCheck <= 0f)
            {
                runMotion = false;
                changeDirectionOfRunningCoolTimeCheck = changeDirectionOfRunningCoolTime;
            }
        }
        else if (xPos > 0 && runMotion)
            changeDirectionOfRunningCoolTimeCheck = changeDirectionOfRunningCoolTime;
    }

    // �÷��̾��� ���¸� �ٲ��ִ� �Լ�
    void PlayerState(float xPos, bool runMotion)
    {
        if (runMotion)
        {
            animator.SetFloat("Move", (float)ePlayerState.Run);
            speed = data.runSpeed;
        }
        else
        {
            if (xPos != 0)
            {
                animator.SetFloat("Move", (float)ePlayerState.Walk);
                speed = data.walkSpeed;
            }
            else if (xPos == 0)
            {
                animator.SetFloat("Move", (float)ePlayerState.Idle);
                speed = 0;
            }
        }
    }

    // �÷��̾ �¿� �ٶ󺸰� ���ִ� �Լ�
    void PlayerDirection(float xPos)
    {
        if (xPos > 0)
            transform.localRotation = Quaternion.Euler(0, 90, 0);
        else if (xPos < 0)
            transform.localRotation = Quaternion.Euler(0, -90, 0);
    }

    // �޸��⸦ �Ϸ��� ���� ����Ű�� 0.5�ʾȿ� ���� 2�� ������ �Ѵ�
    void PlayerRunMotionCheck(float xPos)
    {
        if (groundCheck.JumpingMotion)
            return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            rightRunMotionCount++;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            leftRunMotionCount++;

        if (rightRunMotionCount == 1 || leftRunMotionCount == 1)
        {
            changeRunModeCoolTime -= Time.deltaTime;
            //print("�� ������: " + runModeCoolTime);
            if (changeRunModeCoolTime <= 0f)
            {
                rightRunMotionCount = 0;
                leftRunMotionCount = 0;
                changeRunModeCoolTime = 0.5f;
            }
        }

        if (changeRunModeCoolTime > 0f && (rightRunMotionCount == 2 || leftRunMotionCount == 2))
        {
            //print("�����: " + runModeCoolTime);
            runMotion = true;
            rightRunMotionCount = 0;
            leftRunMotionCount = 0;
            changeRunModeCoolTime = 0.5f;
        }
    }

    public void JumpMotion()
    {
        if (state == ePlayerState.Attack)
            return;

        SoundManager.Instance.PlaySFX(Sfx.Jump);
        rig.AddForce(Vector3.up * data.jumpPower, ForceMode.Impulse);
    }

    // �������� �¾��� �� �ʱ�ȭ �ؾ��� �͵� 
    void InitState()
    {
        damaged = true;
        speed = 0;
        runMotion = false;
        skilling = false;
        animator.SetFloat("Move", (float)ePlayerState.Idle);
    }

    // DamagedFront, DamagedBack ��ǿ� �̺�Ʈ �Լ��� ��ϵ�����
    void DamageEnd()
    {
        damaged = false;
    }

    void FootSound()
    {
        Scene scene = SceneManager.GetSceneByName("Stage");
        if(scene.name == "Stage")
            SoundManager.Instance.PlayStageFoot();
        else
            SoundManager.Instance.PlayBossStageFoot();
    }
}
