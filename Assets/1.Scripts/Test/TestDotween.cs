using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class TestDotween : MonoBehaviour
{

    [SerializeField]
    private GameObject monster = null;
    private List<int> prices = new List<int>();
    //private CinemachineVirtualCamera virtualCamera = null;
    private static TestDotween _instance;
    public static TestDotween Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameObject("Manager").AddComponent<TestDotween>();
            return _instance;
        }
    }

    private void Start()
    {
        //prices[0] = 100;
    }
}
