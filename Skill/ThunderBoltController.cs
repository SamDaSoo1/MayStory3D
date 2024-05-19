using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ThunderBoltController: MonoBehaviour
{
    [SerializeField] List<GameObject> players;
    [SerializeField] GameObject thunder_Prodromal_Symptoms;
    [SerializeField] GameObject thunder_Effect;
    [SerializeField] GameObject thunderBolt;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Collider _collider;

    private void Awake()
    {
        thunder_Prodromal_Symptoms.SetActive(false);
        thunder_Effect.SetActive(false);
        lineRenderer = thunderBolt.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        _collider = thunderBolt.GetComponent<CapsuleCollider>();
        _collider.enabled = false;
    }


    public void Strike(Transform monster)
    {
        StartCoroutine(OnOff(monster));
    }

    IEnumerator OnOff(Transform monster)
    {
        int count = 5;
        while(count > 0)
        {
            SoundManager.Instance.PlayBossSfx(BossSfx.BossSkill_thunder);
            GameObject player = null;
            int ranNum = Random.Range(-3, 4);
            foreach(GameObject p in players)
            {
                if (p.activeSelf)
                    player = p;
            }
            thunder_Prodromal_Symptoms.SetActive(true);
            thunder_Prodromal_Symptoms.transform.position = new Vector3(ranNum + player.transform.position.x, player.transform.position.y + 5f, 0);
            thunderBolt.transform.position = new Vector3(ranNum - 2 + player.transform.position.x, player.transform.position.y + 1f, 0);
            thunder_Effect.transform.position = new Vector3(ranNum + player.transform.position.x, player.transform.position.y, 0);
            yield return new WaitForSeconds(1);
            lineRenderer.enabled = true;
            _collider.enabled = true;
            thunder_Effect.SetActive(true);
            yield return new WaitForSeconds(1);
            thunder_Prodromal_Symptoms.SetActive(false);
            _collider.enabled = false;
            lineRenderer.enabled = false;
            thunder_Effect.SetActive(false);
            count--;
        }
    }
}
