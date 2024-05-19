using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ToyHammer : MonoBehaviour
{
    [SerializeField] GameObject particle;
    [SerializeField] WeaponSwitching ws;

    void Start()
    {
        ws = GameObject.FindObjectOfType<WeaponSwitching>();
        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        float time = 0f;
        while (true)
        {
            yield return null;
            time -= Time.deltaTime * 200;
            transform.rotation = Quaternion.Euler(-90, 0, time);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ws.GetHammer();

            particle.SetActive(false);
            gameObject.SetActive(false); 
        }
    }
}
