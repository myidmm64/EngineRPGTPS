using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestDotween : MonoBehaviour
{

    private void Start()
    {

        transform.DOMoveX(2f, 3f).OnComplete(DDDD);
    }
    private void DDDD()
    {
        Debug.Log("asds");
    }
}
