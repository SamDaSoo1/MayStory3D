using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    [SerializeField] GameObject[] players;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] Image loading;
    [SerializeField] TextMeshProUGUI warningText;
    [SerializeField] PlayerTag playerTag;

    Color color;
    bool isOpen = false;
    public bool DownArrowClick = false;
    Coroutine co;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = true;
        loading.enabled = false;
        color = warningText.color;
        warningText.color = Color.clear;
    }

    private void Start()
    {
        playerTag = GameObject.FindObjectOfType<PlayerTag>();
    }

    public void Open()
    {
        isOpen = true;
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.P))
        {
            StartCoroutine(FadeIn());
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            if(co != null)
                StopCoroutine(co);
            co = StartCoroutine(TextFadeOut());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && (Input.GetKeyDown(KeyCode.DownArrow) || DownArrowClick) && isOpen)
        {
            StartCoroutine(FadeIn());
            DownArrowClick = false;
        }
        else if(other.CompareTag("Player") && (Input.GetKeyDown(KeyCode.DownArrow) || DownArrowClick) && !isOpen)
        {
            DownArrowClick = false;
            if (co != null)
                StopCoroutine(co);
            StartCoroutine(TextFadeOut());
        }
    }

    IEnumerator TextFadeOut()
    {
        SoundManager.Instance.PlaySFX(Sfx.PortalFailed);
        float time = 0f;
        warningText.color = color;
        while (time < 2f)
        {
            yield return null;
            time += Time.deltaTime;
            if(time > 1f)
            {
                warningText.color = new Color(224/255f, 14/255f, 0f, (2 - time) / 1f);
            }
            loading.color = Color.clear;
        }
    }

    IEnumerator FadeIn()
    {
        SoundManager.Instance.PlaySFX(Sfx.Portal);
        float time = 0f;
        loading.enabled = true;
        while (time < 1f)
        {
            yield return null;
            time += Time.deltaTime;
            loading.color = new Color(0, 0, 0,  time / 1f);
        }

        loading.color = new Color(0, 0, 0, 1);

        PlayerPrefs.SetInt("Warrior", playerTag._hp[0]);
        PlayerPrefs.SetInt("Dagger", playerTag._hp[1]);

        if (players[0].activeSelf)
            PlayerPrefs.SetInt("Player", 0);
        else if (players[1].activeSelf)
            PlayerPrefs.SetInt("Player", 1);

        SceneManager.LoadScene("LoadingScene");
    }
}
