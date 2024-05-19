using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearText : MonoBehaviour
{
    [SerializeField] List<GameObject> players;
    [SerializeField] GameObject clearText;
    [SerializeField] Image loading;

    private void Start()
    {
        GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void CoStart()
    {
        StartCoroutine(_ClearText());
    }

    IEnumerator _ClearText()
    {
        yield return new WaitForSeconds(2.5f);
        SoundManager.Instance.PlaySFX(Sfx.Clear);
        clearText.GetComponent<RectTransform>().DOScale(1.5f, 0.3f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(0.3f);
        clearText.GetComponent<RectTransform>().DOScale(1f, 0.3f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(1f);
        float time = 0f;
        while (time < 1f)
        {
            yield return null;
            time += Time.deltaTime / 2;
            clearText.GetComponent<Image>().color = new Color(1, 1, 1, 1 - time);
        }
        clearText.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(1.7f);
        float time2 = 0f;
        loading.enabled = true;
        while (time2 < 1f)
        {
            yield return null;
            time2 += Time.deltaTime;
            loading.color = new Color(0, 0, 0, time2);
        }

        if (players[0].activeSelf)
            PlayerPrefs.SetInt("Player", 0);
        else if (players[1].activeSelf)
            PlayerPrefs.SetInt("Player", 1);

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Clear Stage");
    }
}
