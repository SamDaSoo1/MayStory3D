using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MonsterNavBase : MonoBehaviour
{
    public Vector3 center;
    public Vector3 posTarget;
    Vector3 arrival_Point;
    protected int key;

    public eMonsterState monsterState;

    [SerializeField] protected List<PlayerInfoBase> playerInfo;
    [SerializeField] protected List<Transform> player;

    BoxCollider boxCollider;
    protected Animator animator;
    protected NavMeshAgent agent;
    protected readonly float limitX = 0.5f;
    protected float stateSelectCoolTime = 2f;
    protected float xPos = 0f;
    protected float attackRange; 
    protected bool attacking = false;
    
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
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        boxCollider = GetComponent<BoxCollider>();
        agent.enabled = false;
        posTarget = transform.position;
        center = transform.position;
    }

    void Start()
    {
        //GameObject[] go = GameObject.FindGameObjectsWithTag("Player");
        //playerInfo.Add(go[0].GetComponent<PlayerInfoBase>());
        //playerInfo.Add(go[1].GetComponent<PlayerInfoBase>());
        //player.Add(go[0].transform);
        //player.Add(go[1].transform);

        attackRange = DataManager.instance.GetMonsterData(key).attackRange;
        agent.enabled = true;
    }

    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            return;

        if (monsterState == eMonsterState.Angry)
        {
            DistanceCheck();
        }
    }

    void Update()
    {
        if (monsterState == eMonsterState.Angry || animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            return;

        if (monsterState == eMonsterState.Die)
            agent.isStopped = true;

        if (gameObject.name != "Flower Dryad")
            MonsterAI();
    }

    public void SetMonsterState(eMonsterState state)
    {
        monsterState = state;
    }

    // 스스로 행동을 정하는 똑똑한 몬스터
    void MonsterAI() 
    {
        stateSelectCoolTime -= Time.deltaTime;

        if (stateSelectCoolTime <= 0f)
        {
            stateSelectCoolTime = 2f;
            ProcessAI();
        }
        //print($"{posTarget}, {transform.position}");
        //print(Vector3.Distance(posTarget, transform.position));

        if (Vector3.Distance(posTarget, transform.position) <= 0.1f)
            animator.SetBool("Move", false);
    }

    // 평상시에 가만히 있을지 돌아다닐지 결정해주는 함수
    void ProcessAI() 
    {
        int ranNum = Random.Range(0, 10);
        //print("나온 숫자: " + ranNum);
        if (ranNum < 5)
        {
            monsterState = eMonsterState.Idle;
            //print("평상시");
        }
        else if (ranNum >= 5)
        {
            monsterState = eMonsterState.Move;
            //print("무빙");
        }

        switch (monsterState)
        {
            case eMonsterState.Move:
                posTarget = GetRandomPos(center, limitX);
                animator.SetBool("Move", true);

                Vector3 dir = posTarget - transform.position;
                //print(dir);

                transform.localRotation = Quaternion.LookRotation(dir.normalized);

                agent.SetDestination(posTarget);
                break;
        }
    }

    // 평상시에 돌아다닌다고 정해졌다면 도착지점을 뽑아준다
    Vector3 GetRandomPos(Vector3 center, float limitX)
    {
        xPos = Random.Range(-limitX, limitX);

        Vector3 randomPos = new Vector3(xPos, 0, 0);

        return center + randomPos;
    }

    // 플레이어를 감지하고 몬스터는 화난 상태가 되며 플레이어를 바라봄
    public void PlayerDetection(Transform _player)
    {
        if(_player.name == "Warrior")
        {
            if (player[0] == null)
                player[0] = _player;
            //print("플레이어 감지");
            if (monsterState != eMonsterState.Angry)
                monsterState = eMonsterState.Angry;

            if (player[0].rotation.eulerAngles.y == transform.rotation.eulerAngles.y)
            {
                if (transform.rotation.eulerAngles.y == 270)
                    transform.DORotate(new Vector3(0, 90, 0), 0.5f);
                else if (transform.rotation.eulerAngles.y == 90)
                    transform.DORotate(new Vector3(0, -90, 0), 0.5f);
            }
        }
        else
        {
            if (player[1] == null)
                player[1] = _player;
            //print("플레이어 감지");
            if (monsterState != eMonsterState.Angry)
                monsterState = eMonsterState.Angry;

            if (player[1].rotation.eulerAngles.y == transform.rotation.eulerAngles.y)
            {
                if (transform.rotation.eulerAngles.y == 270)
                    transform.DORotate(new Vector3(0, 90, 0), 0.5f);
                else if (transform.rotation.eulerAngles.y == 90)
                    transform.DORotate(new Vector3(0, -90, 0), 0.5f);
            }
        }
    }

    // 화난 상태일 때 n미터이하에 플레이어가 있다면 공격
    // 플레이어가 n미터를 벗어나면 공격을 멈추고 쫓아감
    void DistanceCheck()
    {
        if (playerInfo[0].gameObject.activeSelf == true)
        {
            if (playerInfo[0].GetPlayerState() == ePlayerState.Dead)
            {
                monsterState = eMonsterState.Idle;
                //print("화 풀림");
                agent.isStopped = false;
                stateSelectCoolTime = 4f;
            }
            else if (Vector3.Distance(player[0].position, transform.position) <= attackRange)
            {
                boxCollider.isTrigger = false;
                agent.isStopped = true;
                animator.SetBool("Move", false);
            }
            else if (Vector3.Distance(player[0].position, transform.position) > attackRange && !attacking)
            {
                boxCollider.isTrigger = true;
                agent.isStopped = false;
                animator.SetBool("Move", true);
                //arrival_Point = player[0].position;
                agent.SetDestination(player[0].position);
            }

            // 플레이어가 반대방향으로 가면 몬스터도 뒤돌아보는 로직
            if (player[0].position.x - transform.position.x < 0)
                transform.DORotate(new Vector3(0, -90, 0), 0.5f);
            else
                transform.DORotate(new Vector3(0, 90, 0), 0.5f);
        }
        else if (playerInfo[1].gameObject.activeSelf == true)
        {
            if (playerInfo[1].GetPlayerState() == ePlayerState.Dead)
            {
                monsterState = eMonsterState.Idle;
                //print("화 풀림");
                agent.isStopped = false;
                stateSelectCoolTime = 4f;
            }
            else if (Vector3.Distance(player[1].position, transform.position) <= attackRange)
            {
                boxCollider.isTrigger = false;
                agent.isStopped = true;
                animator.SetBool("Move", false);
            }
            else if (Vector3.Distance(player[1].position, transform.position) > attackRange && !attacking)
            {
                boxCollider.isTrigger = true;
                agent.isStopped = false;
                animator.SetBool("Move", true);
                //arrival_Point = player[1].position;
                agent.SetDestination(player[1].position);
            }

            // 플레이어가 반대방향으로 가면 몬스터도 뒤돌아보는 로직
            if (player[1].position.x - transform.position.x < 0)
                transform.DORotate(new Vector3(0, -90, 0), 0.5f);
            else
                transform.DORotate(new Vector3(0, 90, 0), 0.5f);
        }
    }

    // 공격 중에는 플레이어를 추적하지 않게함
    void AttackStart()
    {
        //print("공격시작");
        attacking = true;
        GetComponent<MonsterActionBase>().isAttack = false;
    }

    // 공격이 끝나면 플레이어를 다시 추적시작
    void AttackEnd()
    {
        //print("공격끝");
        attacking = false;
    }
}
