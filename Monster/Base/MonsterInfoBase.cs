using System.Collections;
using UnityEngine;

public class MonsterInfoBase : MonoBehaviour
{
    protected int key;
    Animator animator;
    BoxCollider boxCollider;

    public int AttackPower { get; set; }

    int hp; 
    public int Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp <= 0)
            {
                if (gameObject.name == "Turnipa_Bitter")
                    Invoke("Die", 0.5f);
                else if (gameObject.name == "Flower Dryad")
                {
                    animator.Play("Damage", -1, 0);
                    Invoke("BossDie", 0.5f);
                }

                hp = 0;

                if(FindObjectOfType<MonsterCount>() != null)
                    FindObjectOfType<MonsterCount>().TextUpdate();

                GetComponent<MonsterNavBase>().SetMonsterState(eMonsterState.Die);
                animator.SetTrigger("Die");
                StartCoroutine(AfterDeath());
            }
        }
    }

    void BossDie()
    {
        SoundManager.Instance.PlayBossSfx(BossSfx.BossDie);
    }

    void Die()
    {
        SoundManager.Instance.PlayMobSfx(MobSfx.MobDie);
    }

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    protected virtual void Start()
    {
        AttackPower = DataManager.instance.GetMonsterData(key).attackPower;
        Hp = DataManager.instance.GetMonsterData(key).hp;
        //print(hp);
    }

    IEnumerator AfterDeath()
    {
        yield return null;
        boxCollider.enabled = false;
        //StartCoroutine(dropItem.DropItemCo());       // 일단 템드랍 막아놓음
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
