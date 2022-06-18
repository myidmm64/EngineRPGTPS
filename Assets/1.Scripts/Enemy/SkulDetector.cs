using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스컬의 디텍터
public class SkulDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) // 만약 디텍터가 플레이어에 닿았다면 추적 시작
        {
            SendMessageUpwards("OnCkTarget", other.gameObject);
        }
    }
}
