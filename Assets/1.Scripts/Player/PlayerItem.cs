using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerItem : MonoBehaviour
{
    [field: SerializeField]
    private UnityEvent OnHPPosion = null;
    [field: SerializeField]
    private UnityEvent OnMPPosion = null;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            OnHPPosion?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OnMPPosion?.Invoke();
        }
    }
}
