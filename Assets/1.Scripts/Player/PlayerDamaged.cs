using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamaged : MonoBehaviour
{
    [SerializeField]
    private int _hp = 10;
    [SerializeField]
    private float _damageDelay = 1f; // 맞는 딜레이

    private bool _isDamage = false; // 현재 맞고있는가
    private Animator _animator = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAtk"))
        {
            if (_isDamage == false)
            {
                StartCoroutine(DamageCoroutine());
            }
        }
    }

    private IEnumerator DamageCoroutine()
    {
        _isDamage = true;
        _animator.SetTrigger("Damage");
        Debug.Log("맞았어요");
        yield return new WaitForSeconds(_damageDelay);
        _isDamage = false;
    }
}
