using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Respown : MonoBehaviour
{
    [SerializeField] GameObject[] players;
    [SerializeField] GameObject particle;

    Vector3 respownArea = new Vector3(57, 0, 0);

    private void Start()
    {
        particle.transform.position = respownArea;
        particle.SetActive(false);
    }

    public void Set()
    {
        foreach (GameObject player in players)
        {
            player.transform.position = respownArea;
        }
        particle.SetActive(true);
        StartCoroutine(ParticleOff());
    }

    IEnumerator ParticleOff()
    {
        yield return new WaitForSeconds(3f);
        particle.SetActive(false);
    }
}
