using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//아이템 줍기 기능
public class PickUpItem : MonoBehaviour
{
    private Player _player = null; // 플레이어 캐싱 준비

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin")) // 만약 코인에 닿았다면 코인 증가.
        {
            other.transform.DOKill();

            _player.Coin++;

            Destroy(other.gameObject);
        }

    }
}
