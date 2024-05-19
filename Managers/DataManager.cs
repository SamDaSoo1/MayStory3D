using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerData
{   
    public int key;                                 // Ű ��
    public string name;                             // �̸�
    public int hp;                                  // ü��
    public int offense_power;                       // ĳ���� ���ݷ�
    public float attack_speed;                      // ���ݼӵ�
    public float basic_Attack;                      // ��Ÿ ������
    public int defense;                             // ����
    public float walkSpeed;                         // �ȴ� �ӵ�
    public float runSpeed;                          // �޸��� �ӵ�
    public float jumpPower;                         // ������
    public float critical_damage;                   // ũ��
    public float movement_Distance_During_Attack;   // ��Ÿ���� �ι�° ��� ���� �Ÿ�
    public float movement_Time_During_Attack;       // ��Ÿ���� �ι�° ��� ���� �ð�
    public float knockBackDistance;                 // �˹� �Ÿ� 
    public float auto_StandUp_Time;                 // �ڵ����ð�
    public float skillA_Duration_Time;              // ��ųA ���ӽð�
    public float skillA_DelayTime_AfterAction;      // ��ųA �ĵ�
    public float skillA_Effect_SwordSizeUp;         // ��ųA ȿ��
    public float skillA_CoolDown_Time;              // ��ųA ��Ÿ��
    public float skillB_Damage;                     // ��ųB ������
    public int SkillB_NumberOfHits;                 // ��ųB Ÿ��
    public float skillB_DelayTime_AfterAction;      // ��ųB �ĵ�
    public float skillB_CoolDown_Time;              // ��ųB ��Ÿ��
    public float skillC_Damage;                     // ��ųC ������
    public int SkillC_NumberOfHits;                 // ��ųC Ÿ��
    public float skillC_CoolDown_Time;              // ��ųC ��Ÿ��
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
        //print("�����͸Ŵ���");
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
