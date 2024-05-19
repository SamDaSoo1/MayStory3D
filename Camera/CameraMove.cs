using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Respown respown;
    GameObject player;
    Vector3 camPos = new Vector3(0, 1, -5);
    Vector3 camOffset = new Vector3(0, 1, -5);

    public bool isTag { get; set; } = false;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        transform.position = camPos;
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM(BgmSound.StageBgm);
        respown = GameObject.FindObjectOfType<Respown>();
        player = GameObject.Find("Warrior");
    }

    void LateUpdate()
    {
        if (isTag) { return; }

        transform.position = player.transform.position + camOffset;

        if (transform.position.y < -3f)
            respown.Set();
    }

    public void ChangeFocus(GameObject _player)
    {
        player = _player;
    }
}
