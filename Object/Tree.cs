using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] List<Material> treeColor;
    Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        int ranNum = Random.Range(0, 4);
        _renderer.material = treeColor[ranNum];
    }
}
