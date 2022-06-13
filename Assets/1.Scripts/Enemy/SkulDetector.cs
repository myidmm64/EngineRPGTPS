using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("적 감지 !! 이름 : " + other.name);
            SendMessageUpwards("OnCkTarget", other.gameObject);
        }
    }
}
