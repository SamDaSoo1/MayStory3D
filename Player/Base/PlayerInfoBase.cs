using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfoBase : MonoBehaviour
{
    [SerializeField] Transform monster;
    [SerializeField] LoadingSceneEnd loadingSceneEnd;

    public bool stun = false;
    [HideInInspector] public int offense_power;

    [SerializeField] protected PlayerHpBar playerHpBar;
    public Animator animator;
    Rigidbody rig;
    BoxCollider boxCollider;

    public ePlayerState state;

    protected PlayerData data;

    public float maxHp;
    bool hp25down = false;
    bool hp50down = false;
    bool hp75down = false;


    int hp;
    public int Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp <= 0)
            {
                SoundManager.Instance.PlayPlayerSfx(PlayerSfx.PlayerDead);

                hp = 0;
                animator.SetTrigger("Die");
                state = ePlayerState.Dead;
                if (PlayerPrefs.HasKey("Warrior"))
                    PlayerPrefs.SetInt("Warrior", 500);
                if (PlayerPrefs.HasKey("Dagger"))
                    PlayerPrefs.SetInt("Dagger", 300);
                StartCoroutine(GameOver());
            }
            else if (hp <= maxHp * 0.25f && !hp25down)
            {
                stun = true;
                hp25down = true;
                hp50down = true;
                hp75down = true;
                StunDirection();
                //print("피25%이하");
            }
            else if (hp <= maxHp * 0.5f && !hp50down)
            {
                stun = true;
                hp50down = true;
                hp75down = true;
                StunDirection();
                //print("피50%이하");
            }
            else if (hp <= maxHp * 0.75f && !hp75down)
            {
                stun = true;
                hp75down = true;
                StunDirection();
                //print("피75%이하");
            }
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3f);
        loadingSceneEnd.StartCoroutine(loadingSceneEnd.FadeIn());
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Main");
    }

    private void Awake()
    {
        playerHpBar = FindObjectOfType<PlayerHpBar>();
        rig = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        state = ePlayerState.Alive;
    }

    public virtual void Start()
    {
        loadingSceneEnd = GameObject.FindObjectOfType<LoadingSceneEnd>();
        maxHp = data.hp;

        Scene scene = SceneManager.GetSceneByName("Boss Stage");
        if (scene.name == "Boss Stage")
        {
            if(gameObject.name == "Warrior")
            {
                Hp = PlayerPrefs.GetInt("Warrior");
            }
            else if (gameObject.name == "Dagger")
            {
                Hp = PlayerPrefs.GetInt("Dagger");
            }  
        }
        else
        {
            Hp = data.hp;
        }
            

        SetHp();
        offense_power = data.offense_power;
    }

    public void SetHp()
    {
        //print("셋업");
        playerHpBar.SliderValueSet(Hp);
    }

    private void Update()
    {
        playerHpBar.SliderValueUpdate(Hp);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Field") && state == ePlayerState.Dead)
        {
            rig.isKinematic = true;
            boxCollider.enabled = false;
        }
    }

    public ePlayerState GetPlayerState()
    {
        return state;
    }

    public void LastMonsterhit(Transform _monster)
    {
        monster = _monster;
    }

    private void StunDirection()
    {
        Vector3 monsterDir = monster.forward.normalized;
        Vector3 playerDir = transform.forward.normalized;

        //print(monsterDir.x + "," + monsterDir.y + "," + monsterDir.z);
        //print(playerDir.x + "," + playerDir.y + "," + playerDir.z);


        if (monsterDir.x.Equals(playerDir.x))
            animator.SetTrigger("DownFront");
        else if (!monsterDir.x.Equals(playerDir.x))
            animator.SetTrigger("DownBack");
    }
}
