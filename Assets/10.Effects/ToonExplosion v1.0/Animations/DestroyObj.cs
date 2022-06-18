using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObj : MonoBehaviour
{
    [SerializeField]
    private Object _destory = null;
    public void DestroyObject()
    {
        Destroy(_destory);
    }
}
