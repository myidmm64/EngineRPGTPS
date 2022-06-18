using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerItem : MonoBehaviour
{
    [field: SerializeField]
    private UnityEvent OnHPPosion = null; // HP포션을 먹었을 때 실행될 이벤트
    [field: SerializeField]
    private UnityEvent OnMPPosion = null; // MP포션을 먹었을 때 실행될 이벤트


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) // T키를 누르면 HP 포션
        {
            OnHPPosion?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Y)) // Y키를 누르면 MP포션
        {
            OnMPPosion?.Invoke();
        }
    }
}
