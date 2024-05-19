using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerData
{   
    public int key;                                 // 키 값
    public string name;                             // 이름
    public int hp;                                  // 체력
    public int offense_power;                       // 캐릭터 공격력
    public float attack_speed;                      // 공격속도
    public float basic_Attack;                      // 평타 데미지
    public int defense;                             // 방어력
    public float walkSpeed;                         // 걷는 속도
    public float runSpeed;                          // 달리는 속도
    public float jumpPower;                         // 점프력
    public float critical_damage;                   // 크뎀
    public float movement_Distance_During_Attack;   // 평타에서 두번째 모션 전진 거리
    public float movement_Time_During_Attack;       // 평타에서 두번째 모션 전진 시간
    public float knockBackDistance;                 // 넉백 거리 
    public float auto_StandUp_Time;                 // 자동기상시간
    public float skillA_Duration_Time;              // 스킬A 지속시간
    public float skillA_DelayTime_AfterAction;      // 스킬A 후딜
    public float skillA_Effect_SwordSizeUp;         // 스킬A 효과
    public float skillA_CoolDown_Time;              // 스킬A 쿨타임
    public float skillB_Damage;                     // 스킬B 데미지
    public int SkillB_NumberOfHits;                 // 스킬B 타수
    public float skillB_DelayTime_AfterAction;      // 스킬B 후딜
    public float skillB_CoolDown_Time;              // 스킬B 쿨타임
    public float skillC_Damage;                     // 스킬C 데미지
    public int SkillC_NumberOfHits;                 // 스킬C 타수
    public float skillC_CoolDown_Time;              // 스킬C 쿨타임
}

public struct MonsterData
{
    public int key;
    public string name;
    public int hp;
    public int attackPower;
    public float attackCoolTime;
    public int skillPower;
    public float skillCoolTime;
    public float knockBackDistance;
    public float attackRange;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    Dictionary<int, PlayerData> dicData = new Dictionary<int, PlayerData>();
    Dictionary<int, MonsterData> dicData2 = new Dictionary<int, MonsterData>();

    private void Awake()
    {
        //print("데이터매니저");
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadPlayerData();
        LoadMonsterData();
    }

    // --------------------------------------------------PlayerData-------------------------------------------------------

    public PlayerData GetPlayerData(int key)
    {
        return dicData[key];
    }

    void LoadPlayerData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("CSV_Data/PlayerData");
        string[] rowData = textAsset.text.Split("\n");

        for(int i = 1; i < rowData.Length; i++)
        {
            string[] colData = rowData[i].Split(',');

            PlayerData playerData = new PlayerData();

            playerData.key                               = int.Parse(colData[0]);
            playerData.name                              = colData[1];
            playerData.hp                                = int.Parse(colData[2]);
            playerData.offense_power                     = int.Parse(colData[3]);
            playerData.attack_speed                      = float.Parse(colData[4]);
            playerData.basic_Attack                      = float.Parse(colData[5]);
            playerData.defense                           = int.Parse(colData[6]);
            playerData.walkSpeed                         = float.Parse(colData[7]);
            playerData.runSpeed                          = float.Parse(colData[8]);
            playerData.jumpPower                         = float.Parse(colData[9]);
            playerData.critical_damage                   = float.Parse(colData[10]);
            playerData.movement_Distance_During_Attack   = float.Parse(colData[11]);
            playerData.movement_Time_During_Attack       = float.Parse(colData[12]);
            playerData.knockBackDistance                 = float.Parse(colData[13]);
            playerData.auto_StandUp_Time                 = float.Parse(colData[14]);
            playerData.skillA_Duration_Time              = float.Parse(colData[15]);
            playerData.skillA_DelayTime_AfterAction      = float.Parse(colData[16]);
            playerData.skillA_Effect_SwordSizeUp         = float.Parse(colData[17]);
            playerData.skillA_CoolDown_Time              = float.Parse(colData[18]);
            playerData.skillB_Damage                     = float.Parse(colData[19]);
            playerData.SkillB_NumberOfHits               = int.Parse(colData[20]);
            playerData.skillB_DelayTime_AfterAction      = float.Parse(colData[21]);
            playerData.skillB_CoolDown_Time              = float.Parse(colData[22]);
            playerData.skillC_Damage                     = float.Parse(colData[23]);
            playerData.SkillC_NumberOfHits               = int.Parse(colData[24]);
            playerData.skillC_CoolDown_Time              = float.Parse(colData[25]);

            dicData[playerData.key] = playerData;
        }
    }

    // --------------------------------------------------MonsterData-------------------------------------------------------

    public MonsterData GetMonsterData(int key)
    {
        return dicData2[key];
    }

    void LoadMonsterData()
    {

        TextAsset textAsset = Resources.Load<TextAsset>("CSV_Data/MonsterData");
        string[] rowData = textAsset.text.Split("\n");

        for (int i = 1; i < rowData.Length; i++)
        {
            string[] colData = rowData[i].Split(',');

            MonsterData monsterData = new MonsterData();

            monsterData.key                  = int.Parse(colData[0]);
            monsterData.name                 = colData[1];
            monsterData.hp                   = int.Parse(colData[2]);
            monsterData.attackPower          = int.Parse(colData[3]);
            monsterData.attackCoolTime       = float.Parse(colData[4]);
            monsterData.skillPower           = int.Parse(colData[5]);
            monsterData.skillCoolTime        = float.Parse(colData[6]);
            monsterData.knockBackDistance    = float.Parse(colData[7]);
            monsterData.attackRange          = float.Parse(colData[8]);

            dicData2[monsterData.key] = monsterData;
        }
    }
}
