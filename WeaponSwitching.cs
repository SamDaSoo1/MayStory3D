using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponSwitching : MonoBehaviour
{
    [SerializeField] List<GameObject> players;
    [SerializeField] GameObject R_Knife;
    [SerializeField] GameObject L_Knife;
    [SerializeField] GameObject R_Hammer;
    [SerializeField] GameObject L_Hammer;

    [SerializeField] GameObject Sword;
    [SerializeField] GameObject SpearFish;

    private void Start()
    {
        Scene scene = SceneManager.GetSceneByName("Clear Stage");
        if (scene.name == "Clear Stage")
        {
            if (PlayerPrefs.GetInt("Player") == 0)
            {
                players[1].SetActive(false);
                players[0].SetActive(true);
            }
            else if (PlayerPrefs.GetInt("Player") == 1)
            {
                players[0].SetActive(false);
                players[1].SetActive(true);
            }
        } 

        if (PlayerPrefs.HasKey("DaggerSwap"))
        {
            if (PlayerPrefs.GetInt("DaggerSwap") == 0)
            {
                R_Knife.SetActive(true);
                L_Knife.SetActive(true);
                R_Hammer.SetActive(false);
                L_Hammer.SetActive(false);
            }
            else if (PlayerPrefs.GetInt("DaggerSwap") == 1)
            {
                R_Knife.SetActive(false);
                L_Knife.SetActive(false);
                R_Hammer.SetActive(true);
                L_Hammer.SetActive(true);
            } 
        }

        if (PlayerPrefs.HasKey("WarriorSwap"))
        {
            if (PlayerPrefs.GetInt("WarriorSwap") == 0)
            {
                Sword.SetActive(true);
                SpearFish.SetActive(false);
            }
            else if (PlayerPrefs.GetInt("WarriorSwap") == 1)
            {
                Sword.SetActive(false);
                SpearFish.SetActive(true);
            }
        }
    }

    void Update()
    {
        if (players[1].activeSelf && PlayerPrefs.GetInt("Hammer") == 1 && Input.GetKeyDown(KeyCode.Alpha1))
        {
            SoundManager.Instance.PlaySFX(Sfx.WeaponSwap);

            PlayerPrefs.SetInt("DaggerSwap", 0);

            R_Knife.SetActive(true);
            L_Knife.SetActive(true);
            R_Hammer.SetActive(false);
            L_Hammer.SetActive(false);
        }

        if (players[1].activeSelf && PlayerPrefs.GetInt("Hammer") == 1 && Input.GetKeyDown(KeyCode.Alpha2))
        {
            SoundManager.Instance.PlaySFX(Sfx.WeaponSwap);

            PlayerPrefs.SetInt("DaggerSwap", 1);

            R_Knife.SetActive(false);
            L_Knife.SetActive(false);
            R_Hammer.SetActive(true);
            L_Hammer.SetActive(true);
        }

        if (players[0].activeSelf && PlayerPrefs.GetInt("SpearFish") == 1 && Input.GetKeyDown(KeyCode.Alpha1))
        {
            SoundManager.Instance.PlaySFX(Sfx.WeaponSwap);

            PlayerPrefs.SetInt("WarriorSwap", 0);

            Sword.SetActive(true);
            SpearFish.SetActive(false);
        }

        if (players[0].activeSelf && PlayerPrefs.GetInt("SpearFish") == 1 && Input.GetKeyDown(KeyCode.Alpha2))
        {
            SoundManager.Instance.PlaySFX(Sfx.WeaponSwap);

            PlayerPrefs.SetInt("WarriorSwap", 1);

            Sword.SetActive(false);
            SpearFish.SetActive(true);
        }
    }

    public void GetHammer()
    {
        SoundManager.Instance.PlaySFX(Sfx.HammerGet);
        PlayerPrefs.SetInt("Hammer", 1);
    }
}
