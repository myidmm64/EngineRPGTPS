using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("�� ���� !! �̸� : " + other.name);
            SendMessageUpwards("OnCkTarget", other.gameObject);
        }
    }
}
