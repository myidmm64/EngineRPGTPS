using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickUpItem : MonoBehaviour
{
    private Player _player = null;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            other.transform.DOKill();

            _player.Coin++;

            Destroy(other.gameObject);
        }

    }
}
