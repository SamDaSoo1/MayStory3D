using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rank : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Image rankImg;
    [SerializeField] List<Sprite> rankImgList;
    [SerializeField] Image loading;

    void Start()
    {
        btn.interactable = false;
        SoundManager.Instance.PlayBGM(BgmSound.ClearStageBgm);

        timeText.text = "";
        rankImg.color = new Color(1, 1, 1, 0);
        rankImg.enabled = false;
        rankImgList = new List<Sprite>()
        {
            Resources.Load<Sprite>("UI/13/S"),
            Resources.Load<Sprite>("UI/13/A"),
            Resources.Load<Sprite>("UI/13/B"),
            Resources.Load<Sprite>("UI/13/C"),
            Resources.Load<Sprite>("UI/13/D")
        };

        StartCoroutine(Result());
    }

    IEnumerator Result()
    {
        float time = 0f;
        yield return new WaitForSeconds(5f);
        SoundManager.Instance.PlaySFX(Sfx.ClearTime);
        while(true)
        {
            yield return null;
            time += Time.deltaTime * 50f;
            if(time > PlayerPrefs.GetFloat("ClearTime"))
            {
                timeText.text = string.Format("클리어 시간 : {0:N}초", PlayerPrefs.GetFloat("ClearTime"));
                break;
            }
            timeText.text = string.Format("클리어 시간 : {0:N}초", time);
        }
        SoundManager.Instance.AllStopSFX();

        yield return new WaitForSeconds(1f);
        SoundManager.Instance.PlaySFX(Sfx.Rank);

        rankImg.enabled = true;
        if (PlayerPrefs.GetFloat("ClearTime") < 75f)
        {
            rankImg.sprite = rankImgList[0];
            FindObjectOfType<PlayerInfoBase>().animator.SetTrigger("HighScore");
        }
        else if (PlayerPrefs.GetFloat("ClearTime") < 80f)
        {
            rankImg.sprite = rankImgList[1];
            FindObjectOfType<PlayerInfoBase>().animator.SetTrigger("NormalScore");
        }
        else if (PlayerPrefs.GetFloat("ClearTime") < 85f)
        {
            rankImg.sprite = rankImgList[2];
            FindObjectOfType<PlayerInfoBase>().animator.SetTrigger("NormalScore");
        }
        else if (PlayerPrefs.GetFloat("ClearTime") < 90f)
        {
            rankImg.sprite = rankImgList[3];
            FindObjectOfType<PlayerInfoBase>().animator.SetTrigger("LowScore");
        } 
        else
        {
            rankImg.sprite = rankImgList[4];
            FindObjectOfType<PlayerInfoBase>().animator.SetTrigger("LowScore");
        }

        time = 0f;
        while(time < 1f)
        {
            yield return null;
            time += Time.deltaTime;
            rankImg.color = new Color(1, 1, 1, time);
        }
        rankImg.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(3f);
        btn.interactable = true;
    }

    public void Click()
    {
        StartCoroutine(Main());
    }

    IEnumerator Main()
    {
        loading.enabled = true;
        float time = 0f;
        while (time < 1f)
        {
            yield return null;
            time += Time.deltaTime;
            loading.color = new Color(0, 0, 0, time / 1f);
        }
        loading.color = new Color(0, 0, 0, 1);

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Main");
    }
}
