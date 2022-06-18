
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    private static Camera _mainCam = null;


    public static Camera MainCam
    {
        get
        {
            if (_mainCam == null)
            {
                _mainCam = Camera.main;
            }
            return _mainCam;
        }
    }

    private static Player _player = null;

    public static Player player
    {
        get
        {
            if (_player == null)
            {
                _player = GameObject.Find("Player").GetComponent<Player>();
            }
            return _player;
        }
    }


    private static NightChanger _nightChanger = null;
    public static NightChanger nightChanger
    {
        get
        {
            if (_nightChanger == null)
            {
                _nightChanger = GameObject.Find("WorldLight").GetComponent<NightChanger>();
            }
            return _nightChanger;
        }
    }
}
