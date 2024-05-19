using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerTag : MonoBehaviour
{
    [SerializeField] TagButton tagButton;
    [SerializeField] Image blocking;
    [SerializeField] GameObject particle;
    [SerializeField] List<GameObject> players = new List<GameObject>();
    [SerializeField] List<Sprite> profile = new List<Sprite>();
    [SerializeField] List<Sprite> warrior_SkillImg;
    [SerializeField] List<Sprite> dagger_SkillImg;
    [SerializeField] GameObject canvas1;
    [SerializeField] GameObject canvas2;
    [SerializeField] Image profilePicture;
    [SerializeField] List<Image> skillImg;
    [SerializeField] Slider hpBar;
    [SerializeField] Slider tagGauge;
    [SerializeField] TextMeshProUGUI gaugeText;
    [SerializeField] Image tagProfilePicture;
    [SerializeField] Slider tagHpBar;

    [SerializeField] CameraMove camMove;
    [SerializeField] BossScene_CameraMove camMove2;

    GameObject curPlayer = null;

    public bool Tagging { get; private set; } = false;
    public bool Jumping { get; set; } = false;
    float tagCoolTime;
    float tagCoolTime2 = 5f;
    bool isTag = true;

    public float[] _maxHp;
    public int[] _hp;

    float time = 0f;

    private void Awake()
    {
        particle.SetActive(false);
        profile.Add(Resources.Load<Sprite>("UI/13/profile1"));
        profile.Add(Resources.Load<Sprite>("UI/13/profile2"));
        warrior_SkillImg = new List<Sprite>
        {
            Resources.Load<Sprite>("UI/5/SkillA"),
            Resources.Load<Sprite>("UI/5/SkillB"),
            Resources.Load<Sprite>("UI/5/SkillC")
        };

        _maxHp = new float[2] { 500f, 300f };

        Scene scene = SceneManager.GetSceneByName("Boss Stage");
        if (scene.name == "Boss Stage")
            _hp = new int[2] { PlayerPrefs.GetInt("Warrior"), PlayerPrefs.GetInt("Dagger") };
        else
            _hp = new int[2] { 500, 300 };

        // TODO: 단도 스킬아이콘 받으면 활성화
        dagger_SkillImg = new List<Sprite>
        {
            Resources.Load<Sprite>("UI/14/SkillA"),
            Resources.Load<Sprite>("UI/14/SkillB"),
            Resources.Load<Sprite>("UI/14/SkillC")
        };
    }

    void Start()
    {
        tagButton = GameObject.FindObjectOfType<TagButton>();
        Scene scene = SceneManager.GetSceneByName("Boss Stage");

        //players = GameObject.FindGameObjectsWithTag("Player").ToList();

        if (scene.name == "Boss Stage")
        {
            if (PlayerPrefs.GetInt("Player") == 0)
            {
                players[1].SetActive(false);
                players[0].SetActive(true);
                curPlayer = players[0]; 
            }
            else if (PlayerPrefs.GetInt("Player") == 1)
            {
                players[0].SetActive(false);
                players[1].SetActive(true);
                curPlayer = players[1];
            }

            camMove2.ChangeFocus(curPlayer);
        }
        else
        {
            foreach (GameObject player in players)
            {
                if (player.name == "Dagger")
                    player.SetActive(false);
                else
                    curPlayer = player;
            }
        }

        camMove = Camera.main.GetComponent<CameraMove>();
        canvas1 = GameObject.Find("Canvas 1");
        profilePicture = canvas1.transform.GetChild(2).GetComponent<Image>();
        hpBar = canvas1.transform.GetChild(0).GetComponent<Slider>();
        tagGauge = canvas1.transform.GetChild(1).GetComponent<Slider>();
        tagGauge.maxValue = tagCoolTime2;
        tagGauge.value = tagCoolTime2;
        gaugeText = canvas1.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        gaugeText.text = "100%";
        canvas2 = GameObject.Find("Canvas 2");
        skillImg = new List<Image>
        {
            canvas2.transform.GetChild(5).GetComponent<Image>(),
            canvas2.transform.GetChild(6).GetComponent<Image>(),
            canvas2.transform.GetChild(7).GetComponent<Image>()
        };

        if (scene.name == "Boss Stage")
        {
            if (PlayerPrefs.GetInt("Player") == 0)
            {
                UI_Change(players[0]);
            } 
            else if (PlayerPrefs.GetInt("Player") == 1)
            {
                UI_Change(players[1]);
            } 
        }
    }

    void Update()
    {
        if (blocking.enabled)
            return;

        if ((Input.GetKeyDown(KeyCode.Tab) || tagButton.TagButtonClick) && !Tagging && !Jumping && isTag 
            && !curPlayer.GetComponent<PlayerActionBase>().attacking
            && !curPlayer.GetComponent<PlayerActionBase>().skilling
            && !curPlayer.GetComponent<PlayerActionBase>().damaged)
        {
            tagButton.TagButtonClick = false;
            Tag();
        }
            

        if (players[0].activeSelf == false)
        {
            time += Time.deltaTime;
            if (time >= 1f)
            {
                time = 0f;
                _hp[0] += 4;
                tagHpBar.value += 4;

                if (_hp[0] > _maxHp[0])
                    _hp[0] = (int)_maxHp[0];
            }
        }
        else if(players[1].activeSelf == false)
        {
            time += Time.deltaTime;
            if (time >= 1f)
            {
                time = 0f;
                _hp[1] += 4;
                tagHpBar.value += 4;

                if (_hp[1] > _maxHp[1])
                    _hp[1] = (int)_maxHp[1];
            }
        }
    }

    void WarriorTagSound()
    {
        SoundManager.Instance.PlayPlayerSfx(PlayerSfx.WarriorSkillC);
    }

    void DaggerTagSound()
    {
        if (PlayerPrefs.GetInt("DaggerSwap") == 0)
            SoundManager.Instance.PlayPlayerSfx(PlayerSfx.DaggerSkillC);
        else
            Invoke("HammerSound", 0.55f);
    }

    void HammerSound()
    {
        SoundManager.Instance.PlayPlayerSfx(PlayerSfx.Hammer);
    }

    void Tag()
    {
        isTag = false;
        SkillButtonEvent go = GameObject.FindObjectOfType<SkillButtonEvent>();
        
        StartCoroutine(TagCoolTime2());
        particle.SetActive(true);

        if (curPlayer.transform.forward == Vector3.right)
        {
            particle.transform.position = new Vector3(curPlayer.transform.position.x - 2f, curPlayer.transform.position.y, 0f);
        }
        else
        {
            particle.transform.position = new Vector3(curPlayer.transform.position.x + 2f, curPlayer.transform.position.y, 0f);
        }
        particle.GetComponent<ParticleSystem>().Play();

        Tagging = true;

        if (camMove != null)
            camMove.isTag = true;
        else
            camMove2.isTag = true;

        
        foreach (GameObject player in players)
        {
            if (player.activeSelf == true)
            {
                if(player.name == "Warrior")
                {
                    player.GetComponent<WarriorAction>().Tag_SwordSizeDown();
                }
                player.GetComponent<PlayerActionBase>().skillA_Use = false;
                player.GetComponent<PlayerActionBase>().skillB_Use = false;
                player.GetComponent<PlayerActionBase>().skillC_Use = false;
                player.SetActive(false);
            }
            else
            {
                if (player.name == "Warrior")
                {
                    SoundManager.Instance.PlayPlayerSfx(PlayerSfx.DaggerSkillA);
                    SoundManager.Instance.PlayWarriorTagVoice();
                    Invoke("WarriorTagSound", 0.55f);
                }
                else
                {
                    SoundManager.Instance.PlayPlayerSfx(PlayerSfx.DaggerSkillA);
                    SoundManager.Instance.PlayDaggerTagVoice();
                    Invoke("DaggerTagSound", 0.55f);
                }

                player.SetActive(true);
                go.Tag_Skill_CoolTime(player);
                //player.GetComponent<PlayerInfoBase>().SetHp();
                if (camMove != null)
                    camMove.ChangeFocus(player);
                else
                    camMove2.ChangeFocus(player);
                player.GetComponent<Animator>().SetTrigger("TagSkill");
                player.transform.rotation = curPlayer.transform.rotation;
                player.transform.position = curPlayer.transform.position + (curPlayer.transform.forward * -2);
                TagSkill(player);
                UI_Change(player);
            }
        }
    }

    void UI_Change(GameObject player)
    {
        if (player.name == "Warrior")
        {
            profilePicture.sprite = profile[0];
            tagProfilePicture.sprite = profile[1];
            hpBar.maxValue = _maxHp[0];
            hpBar.value = _hp[0];
            tagHpBar.maxValue = _maxHp[1];
            tagHpBar.value = _hp[1];

            for(int i = 0; i < skillImg.Count; i++)
            {
                skillImg[i].sprite = warrior_SkillImg[i];
            }
        }
        else
        {
            profilePicture.sprite = profile[1];
            tagProfilePicture.sprite = profile[0];
            hpBar.maxValue = _maxHp[1];
            hpBar.value = _hp[1];
            tagHpBar.maxValue = _maxHp[0];
            tagHpBar.value = _hp[0];

            for (int i = 0; i < skillImg.Count; i++)
            {
                skillImg[i].sprite = dagger_SkillImg[i];
            }
        }        
    }

    void TagSkill(GameObject player)
    {
        
        if (player.name == "Warrior")
        {
            tagCoolTime = 2.3f;
            StartCoroutine(TagCoolTime(player));
            player.transform.DOMoveX(curPlayer.transform.position.x, 1f)
            .SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                curPlayer = player;
                if (camMove != null)
                    camMove.isTag = false;
                else
                    camMove2.isTag = false;
                player.GetComponent<WarriorAction>().TagSkill_Range(player);
            });
        }
        else
        {
            tagCoolTime = 1.8f;
            StartCoroutine(TagCoolTime(player));
            player.transform.DOMoveX(curPlayer.transform.position.x, 1f)
            .SetEase(Ease.InExpo)
            .OnComplete(() =>
            {
                curPlayer = player;
                if (camMove != null)
                    camMove.isTag = false;
                else
                    camMove2.isTag = false;
            });
        }
        
    }

    IEnumerator TagCoolTime(GameObject player)
    {
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(tagCoolTime);
        player.GetComponent<BoxCollider>().enabled = true;
        player.GetComponent<Rigidbody>().useGravity = true;
        Tagging = false;
    }

    IEnumerator TagCoolTime2()
    {
        tagButton.Button_Disable();
        tagGauge.value = 0f;
        gaugeText.text = "0%";
        while (tagGauge.value < tagCoolTime2)
        {
            yield return null;
            tagGauge.value += Time.deltaTime;
            gaugeText.text = Mathf.Round((tagGauge.value * 20)).ToString() + "%";
        }
        tagGauge.value = tagCoolTime2;
        gaugeText.text = "100%";
        tagButton.Button_Onable();
        //yield return new WaitForSeconds(tagCoolTime);
        isTag = true;
    }
}
