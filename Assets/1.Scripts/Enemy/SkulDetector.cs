using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SendMessageUpwards("OnCkTarget", other.gameObject);
        }
    }
}
