using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerDash : MonoBehaviour
{
    //여기에는 대시 관련 코드를 작성하면 됩니당.
    private Animator _animator = null;
    private Player _player = null;
    private CapsuleCollider _capsuleCollider = null;
    [SerializeField]
    private float _coolTime = 2f;
    [SerializeField]
    private float _dashPower = 5f;
    private bool _dashAble = true;

    private float _horizontal = 0f;
    private float _vertical = 0f;
    private Vector3 _dashDirection = Vector3.zero;

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
                Dash();
                StartCoroutine(DashAnimation());
                StartCoroutine(DashCoroutine());
            }
        }
    }

    private void Dash()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        _dashDirection = new Vector3(_horizontal, 0f, Mathf.Clamp(_vertical, 0f, 1f)).normalized;

        if(_player.IsBattle || _player.IsZoom)
        {
            if(_dashDirection == Vector3.zero)
                _dashDirection = transform.forward; // 안 눌렀을 때 디폴트
            else
                _dashDirection = transform.rotation * _dashDirection;

            transform.DOMove(transform.position + _dashDirection * 5f, 0.5f);
        }
        else
            transform.DOMove(transform.position + transform.forward.normalized * 5f, 0.5f);
    }

    private IEnumerator DashAnimation()
    {
        //무적판정 추가
        _player.IsStop = true;
        _player.IsFreeze = true;
        _player.SetState(Player.PlayerState.Dash);
        _capsuleCollider.enabled = false; // 이부분 바꾸기
        _animator.SetTrigger("IsDash");
        yield return new WaitForSeconds(0.5f);
        _player.IsStop = false;
        _player.IsFreeze = false;
        _capsuleCollider.enabled = true;

        if (_player.IsBattle)
            _player.SetState(Player.PlayerState.Battle);
        else if (_player.IsZoom)
            _player.SetState(Player.PlayerState.Zoom);
        else
            _player.SetState(Player.PlayerState.Idle);
    }
    
    private IEnumerator DashCoroutine()
    {
        _dashAble = false;
        yield return new WaitForSeconds(_coolTime);
        _dashAble = true;
    }
}
