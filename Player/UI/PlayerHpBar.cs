using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    [SerializeField] Slider playerHpBar;
    [SerializeField] List<GameObject> players;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] PlayerTag playerTag;

    private void Awake()
    {
        playerHpBar = GetComponentInChildren<Slider>();
    }

    private void OnEnable()
    {
        players = new List<GameObject>()
        {
            GameObject.Find("Player").transform.GetChild(0).gameObject,
            GameObject.Find("Player").transform.GetChild(1).gameObject
        };
        hpText = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SliderValueSet(int playerHp)
    {
        playerHpBar.maxValue = playerHp;
        playerHpBar.value = playerHp;
    }

    public void SliderValueUpdate(int playerHp)
    {
        playerHpBar.value = playerHp;
    }

    public void SliderSet(GameObject player)
    {
        if(player.name == players[0].name)
        {
            playerHpBar.maxValue = playerTag._maxHp[0];
            playerHpBar.value = playerTag._hp[0];
            hpText.text = $"{playerTag._hp[0]}/{playerTag._maxHp[0]}";
        }
        else if (player.name == players[1].name)
        {
            playerHpBar.maxValue = playerTag._maxHp[1];
            playerHpBar.value = playerTag._hp[1];
            hpText.text = $"{playerTag._hp[1]}/{playerTag._maxHp[1]}";
        }

        player.GetComponent<PlayerInfoBase>().Hp = (int)playerHpBar.value;
    }    
}
