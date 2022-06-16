using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    private Transform _playerTransform = null;

    private void Start()
    {
        _playerTransform = GameObject.Find("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == false) return;

        transform.DOKill();

        Player player = _playerTransform.GetComponent<Player>();

        if (player != null)
            player.Coin++;

        Destroy(gameObject);
    }
}
