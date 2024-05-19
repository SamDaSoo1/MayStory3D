using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagButton : MonoBehaviour
{
    [SerializeField] Button button;
    public bool TagButtonClick { get; set; } = false;

    public void OnClick()
    {
        SoundManager.Instance.PlaySFX(Sfx.BtMouseClick);
        TagButtonClick = true;
    }

    public void Button_Disable()
    {
        button.interactable = false;
    }

    public void Button_Onable()
    {
        button.interactable = true;
    }
}
