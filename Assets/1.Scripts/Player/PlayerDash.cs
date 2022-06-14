using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDash : MonoBehaviour
{
    //여기에는 대시 관련 코드를 작성하면 됩니당.
    private Animator _animator = null;
    private Player _player = null;
    private CapsuleCollider _capsuleCollider = null;
    [SerializeField]
    private float _coolTime = 2f;
    private bool _dashAble = true;

    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_player.IsRun || _player.IsZoom)
                return;
            if (_dashAble)
            {
                transform.DOMove(transform.position + transform.forward.normalized * 5f, 0.5f);
                StartCoroutine(DashAnimation());
                StartCoroutine(Dash());
            }
        }
    }

    private IEnumerator DashAnimation()
    {
        _player.IsStop = true;
        _player.IsFreeze = true;
        _player.SetState(Player.PlayerState.Dash);
        _capsuleCollider.enabled = false;
        _animator.SetTrigger("IsDash");
        yield return new WaitForSeconds(0.5f);
        _player.IsStop = false;
        _player.IsFreeze = false;
        _capsuleCollider.enabled = true;
        _player.SetState(Player.PlayerState.Idle);
    }
    
    private IEnumerator Dash()
    {
        _dashAble = false;
        yield return new WaitForSeconds(_coolTime);
        _dashAble = true;
    }
}
