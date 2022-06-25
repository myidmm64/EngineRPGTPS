using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamp : MonoBehaviour
{
    [SerializeField]
    private GameObject _target = null;

    private void LateUpdate()
    {
        transform.position = _target.transform.position;    
    }
}
