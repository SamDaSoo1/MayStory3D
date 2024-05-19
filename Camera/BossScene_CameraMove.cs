using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossScene_CameraMove : MonoBehaviour
{
    GameObject canvas1;
    GameObject canvas2;
    GameObject player;
    Vector3 camOffset;
    [SerializeField] GameObject cam2;
    Image loading;
    GameObject boss;
    
    public bool isTag { get; set; } = false;
    public bool isCamTransition { get; set; } = false;

    float time = 0f;
    [HideInInspector] public bool BattleOn = false;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM(BgmSound.BossBgm);
        canvas1 = GameObject.Find("Canvas 1");
        canvas2 = GameObject.Find("Canvas 2");
        camOffset = new Vector3(0, 4.8f, -8.4f);
        cam2.SetActive(false);
        loading = GameObject.Find("Canvas 3").transform.GetChild(0).GetComponent<Image>();
        boss = GameObject.Find("Flower Dryad");
    }

    void LateUpdate()
    {
        if (isTag)
            return;

        transform.position = player.transform.position + camOffset;
    }

    public void ChangeFocus(GameObject _player)
    {
        player = _player;
    }

    public void Cam2On()
    {
        isCamTransition = true;
        GameObject.FindObjectOfType<PlayerActionBase>().DontMove();
        StartCoroutine(BossSoloShot());
    }

    IEnumerator BossSoloShot()
    {
        loading.enabled = true;
        float time = 0f;
        while(time < 1f)
        {
            yield return null;
            time += Time.deltaTime;
            loading.color = new Color(0, 0, 0, time);
        }

        yield return new WaitForSeconds(0.5f);
        canvas1.SetActive(false);
        canvas2.SetActive(false);
        cam2.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        time = 0f;
        while (time < 1f)
        {
            yield return null;
            time += Time.deltaTime;
            loading.color = new Color(0, 0, 0, 1 - time);
        }
        loading.enabled = false;
        boss.GetComponent<FlowerDryadAction>().animator.SetTrigger("CloseUp");
        yield return new WaitForSeconds(3f);
        loading.enabled = true;
        time = 0f;
        while (time < 1f)
        {
            yield return null;
            time += Time.deltaTime;
            loading.color = new Color(0, 0, 0, time);
        }
        yield return new WaitForSeconds(0.5f);
        canvas1.SetActive(true);
        canvas2.SetActive(true);
        cam2.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        time = 0f;
        while (time < 1f)
        {
            yield return null;
            time += Time.deltaTime;
            loading.color = new Color(0, 0, 0, 1 - time);
        }
        loading.enabled = false;

        yield return new WaitForSeconds(0.2f);
        // 몬스터 추적시작
        BattleOn = true;
        isCamTransition = false;
        boss.GetComponent<FlowerDryadAction>().Chase();
    }

    private void Update()
    {
        if (BattleOn)
            time += Time.deltaTime;
    }

    public void BattleEnd()
    {
        PlayerPrefs.SetFloat("ClearTime", time);
        //print(PlayerPrefs.GetFloat("ClearTime"));
    }
}
