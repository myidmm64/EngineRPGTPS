using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    private static Camera _mainCam = null;
    private static CinemachineVirtualCamera _cmVCam = null;


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
    public static CinemachineVirtualCamera VCam
    {
        get
        {
            if (_cmVCam == null)
            {
                _cmVCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
            }
            return _cmVCam;
        }
    }
    public enum ResourceTypeEnum
    {
        None,
        Health,
        Ammo,
        Coin
    }
}
