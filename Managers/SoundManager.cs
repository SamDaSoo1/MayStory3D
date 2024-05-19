using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BgmSound
{ 
    MainBgm,
    StageBgm,
    BossBgm,
    ClearStageBgm
}

public enum PlayerSfx
{
    WarriorWeapon,
    WarriorSkillA,
    WarriorSkillB,
    WarriorSkillC,
    DaggerWeapon,
    DaggerSkillA,
    DaggerSkillB,
    DaggerSkillC,
    PlayerDamage,
    PlayerDead,
    WarriorTagVoice1,
    WarriorTagVoice2,
    DaggerTagVoice1,
    DaggerTagVoice2,
    Hammer
}

public enum MobSfx
{
    MobDamage,
    MobDie
}

public enum BossSfx
{
    BossAttack,
    BossDamage,
    BossSkill_Start,
    BossSkill_thunder,
    BossDie
}

public enum Sfx
{
    BtMouseClick,
    Clear,
    HammerGet,
    Jump,
    Portal,
    WeaponSwap,
    PortalFailed,
    Rank,
    ClearTime,
    HiddenMode
}

public enum StageFoot
{
    foot1,
    foot2,
    foot3,
    foot4
}

public enum BossStageFoot
{
    foot1,
    foot2,
    foot3,
    foot4,
    foot5,
    foot6
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> BgmList;
    [SerializeField] List<AudioClip> PlayerSfxList;
    [SerializeField] List<AudioClip> MobSfxList;
    [SerializeField] List<AudioClip> BossSfxList;
    [SerializeField] List<AudioClip> SfxList;
    [SerializeField] List<AudioClip> StageFootList;
    [SerializeField] List<AudioClip> BossStageFootList;
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource sfx;

    [SerializeField] List<AudioSource> sfxPool;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else
            Destroy(gameObject);

        BgmList = new List<AudioClip>()
        {
            Resources.Load<AudioClip>("Sound/MainBgm"),
            Resources.Load<AudioClip>("Sound/StageBgm"),
            Resources.Load<AudioClip>("Sound/BossBgm"),
            Resources.Load<AudioClip>("Sound/ClearStageBgm"),
        };

        PlayerSfxList = new List<AudioClip>()
        {
            Resources.Load<AudioClip>("Sound/WarriorWeapon"),
            Resources.Load<AudioClip>("Sound/WarriorSkillA"),
            Resources.Load<AudioClip>("Sound/WarriorSkillB"),
            Resources.Load<AudioClip>("Sound/WarriorSkillC"),
            Resources.Load<AudioClip>("Sound/DaggerWeapon"),
            Resources.Load<AudioClip>("Sound/DaggerSkillA"),
            Resources.Load<AudioClip>("Sound/DaggerSkillB"),
            Resources.Load<AudioClip>("Sound/DaggerSkillC"),
            Resources.Load<AudioClip>("Sound/PlayerDamage"),
            Resources.Load<AudioClip>("Sound/PlayerDead"),
            Resources.Load<AudioClip>("Sound/WarriorTagVoice1"),
            Resources.Load<AudioClip>("Sound/WarriorTagVoice2"),
            Resources.Load<AudioClip>("Sound/DaggerTagVoice1"),
            Resources.Load<AudioClip>("Sound/DaggerTagVoice2"),
            Resources.Load<AudioClip>("Sound/Hammer")
        };

        MobSfxList = new List<AudioClip>()
        {
            Resources.Load<AudioClip>("Sound/MobDamage"),
            Resources.Load<AudioClip>("Sound/MobDie")
        };

        BossSfxList = new List<AudioClip>()
        {
            Resources.Load<AudioClip>("Sound/BossAttack"),
            Resources.Load<AudioClip>("Sound/BossDamage"),
            Resources.Load<AudioClip>("Sound/BossSkill_Start"),
            Resources.Load<AudioClip>("Sound/BossSkill_thunder"),
            Resources.Load<AudioClip>("Sound/BossDie")
        };

        SfxList = new List<AudioClip>()
        {
            Resources.Load<AudioClip>("Sound/BtMouseClick"),
            Resources.Load<AudioClip>("Sound/Clear"),
            Resources.Load<AudioClip>("Sound/HammerGet"),
            Resources.Load<AudioClip>("Sound/Jump"),
            Resources.Load<AudioClip>("Sound/Portal"),
            Resources.Load<AudioClip>("Sound/WeaponSwap"),
            Resources.Load<AudioClip>("Sound/PortalFailed"),
            Resources.Load<AudioClip>("Sound/Rank"),
            Resources.Load<AudioClip>("Sound/ClearTime"),
            Resources.Load<AudioClip>("Sound/HiddenMode")
        };

        StageFootList = new List<AudioClip>()
        {
            Resources.Load<AudioClip>("Sound/StageFoot/foot1"),
            Resources.Load<AudioClip>("Sound/StageFoot/foot2"),
            Resources.Load<AudioClip>("Sound/StageFoot/foot3"),
            Resources.Load<AudioClip>("Sound/StageFoot/foot4")
        };

        BossStageFootList = new List<AudioClip>()
        {
            Resources.Load<AudioClip>("Sound/BossStageFoot/foot1"),
            Resources.Load<AudioClip>("Sound/BossStageFoot/foot2"),
            Resources.Load<AudioClip>("Sound/BossStageFoot/foot3"),
            Resources.Load<AudioClip>("Sound/BossStageFoot/foot4"),
            Resources.Load<AudioClip>("Sound/BossStageFoot/foot5"),
            Resources.Load<AudioClip>("Sound/BossStageFoot/foot6")
        };

        sfxPool = new List<AudioSource>();
    }

    AudioSource GetSFX()
    {
        AudioSource select = null;

        foreach (AudioSource audioSource in sfxPool)
        {
            if (audioSource != null && audioSource.isPlaying == false)
            {
                select = audioSource;
                break;
            }
        }

        if (select == null)
        {
            select = Instantiate(sfx, transform);
            sfxPool.Add(select);
        }
        return select;
    }

    public void PlayBGM(BgmSound type)
    {
        bgm.clip = BgmList[(int)type];
        bgm.Play();
    }

    public void StopBGM()
    {
        bgm.Stop();
    }

    public void PlayPlayerSfx(PlayerSfx type)
    {
        AudioSource sfx = GetSFX();
        sfx.clip = PlayerSfxList[(int)type];
        sfx.Play();
    }

    public void PlayWarriorTagVoice()
    {
        AudioSource sfx = GetSFX();
        int tagSound = Random.Range(10, 12);
        sfx.clip = PlayerSfxList[tagSound];
        sfx.Play();
    }

    public void PlayDaggerTagVoice()
    {
        AudioSource sfx = GetSFX();
        int tagSound = Random.Range(12, 14);
        sfx.clip = PlayerSfxList[tagSound];
        sfx.Play();
    }

    public void PlayMobSfx(MobSfx type)
    {
        AudioSource sfx = GetSFX();
        sfx.clip = MobSfxList[(int)type];
        sfx.Play();
    }

    public void PlayBossSfx(BossSfx type)
    {
        AudioSource sfx = GetSFX();
        sfx.clip = BossSfxList[(int)type];
        sfx.Play();
    }

    public void PlaySFX(Sfx type)
    {
        AudioSource sfx = GetSFX();
        sfx.clip = SfxList[(int)type];
        sfx.Play();
    }

    public void PlayStageFoot()
    {
        AudioSource sfx = GetSFX();
        int footSound = Random.Range((int)StageFoot.foot1, (int)StageFoot.foot4);
        sfx.clip = StageFootList[footSound];
        sfx.Play();
    }

    public void PlayBossStageFoot()
    {
        AudioSource sfx = GetSFX();
        int footSound = Random.Range((int)BossStageFoot.foot1, (int)BossStageFoot.foot6);
        sfx.clip = BossStageFootList[footSound];
        sfx.Play();
    }

    public void AllStopSFX()
    {
        foreach (AudioSource sfx in sfxPool)
        {
            sfx.Stop();
        }
    }

    public void BgmVolumeSettimg(float volume)
    {
        bgm.volume = volume;
    }

    public void SfxVolumeSettimg(float volume)
    {
        sfx.volume = volume;

        foreach (AudioSource sfx in sfxPool)
        {
            sfx.volume = volume;
        }
    }
}
