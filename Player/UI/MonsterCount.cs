using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCount : MonoBehaviour
{
    [SerializeField] List<GameObject> monsters = new List<GameObject>();
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    [SerializeField] Image curCountImg;
    [SerializeField] Image slashImg;
    [SerializeField] Image maxCountImg;
    [SerializeField] Portal portal;

    int maxCount = 0;
    int curCount = 0;

    private void Awake()
    {
        sprites.Add(Resources.Load<Sprite>("UI/4/zero"));
        sprites.Add(Resources.Load<Sprite>("UI/4/one"));
        sprites.Add(Resources.Load<Sprite>("UI/4/two"));
        sprites.Add(Resources.Load<Sprite>("UI/4/three"));
        sprites.Add(Resources.Load<Sprite>("UI/4/four"));
        sprites.Add(Resources.Load<Sprite>("UI/4/five"));
        sprites.Add(Resources.Load<Sprite>("UI/4/six"));
        sprites.Add(Resources.Load<Sprite>("UI/4/seven"));
        sprites.Add(Resources.Load<Sprite>("UI/4/eight"));
        sprites.Add(Resources.Load<Sprite>("UI/4/nine"));

        curCountImg = transform.GetChild(0).gameObject.GetComponent<Image>();
        slashImg = transform.GetChild(1).gameObject.GetComponent<Image>();
        maxCountImg = transform.GetChild(2).gameObject.GetComponent<Image>();
        maxCountImg.sprite = sprites[sprites.Count - 1];
    }

    void Start()
    {
        monsters = GameObject.FindGameObjectsWithTag("Monster").ToList();
        maxCount = monsters.Count;
        portal = GameObject.FindObjectOfType<Portal>();
    }

    public void TextUpdate()
    {
        curCount++;
        curCountImg.sprite = sprites[curCount];
        curCountImg.SetNativeSize();

        if (curCount == maxCount)
        {
            curCountImg.color = Color.green;
            slashImg.color = Color.green;
            maxCountImg.color = Color.green;
            portal.Open();
        }   
    }
}
