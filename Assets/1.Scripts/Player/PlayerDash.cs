using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerDash : MonoBehaviour
{
    private Animator _animator = null; // �ִϸ����� ĳ�� �غ�
    private Player _player = null; // �÷��̾� ĳ�� �غ�
    [SerializeField]
    private float _coolTime = 2f; // ��� ��Ÿ��
    [SerializeField]
    private float _dashPower = 5f; // ��� ��
    private bool _dashAble = true; // ��ð� �����Ѱ�

    private float _horizontal = 0f; // �Է°�
    private float _vertical = 0f; // �Է°�
    private Vector3 _dashDirection = Vector3.zero; // ��� ����

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ����ĳ��Ʈ�� ���ѵα�

            if (_player.IsRun || _player.IsZoom || _player.IsFreeze) // �޸����ְų� �� ���¸� ����
                return;
            if (_dashAble) // ��ð� �����ϸ� ����
            {
                Dash();
                StartCoroutine(DashAnimation());
                StartCoroutine(DashCoroutine());
            }
        }
    }

    /// <summary>
    /// ��ø� �����ϴ� �Լ�
    /// </summary>
    private void Dash()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        _dashDirection = new Vector3(_horizontal, 0f, _vertical);

        if(_player.IsBattle || _player.IsZoom)
        {
            if(_dashDirection == Vector3.zero)
                _dashDirection = transform.forward; // �� ������ �� ����Ʈ������ �չ���
            else
                _dashDirection = transform.rotation * _dashDirection;

            if (_vertical < 0f)
                transform.rotation *= Quaternion.Euler(new Vector3(0f, 180f, 0f));

            transform.DOMove(transform.position + _dashDirection * 5f, 0.5f);
        }
        else
            transform.DOMove(transform.position + transform.forward.normalized * 5f, 0.5f);
    }

    /// <summary>
    /// ��� �ִϸ��̼� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator DashAnimation()
    {
        int layer = _player.gameObject.layer;
        //�������� �߰�
        _player.IsStop = true;
        _player.IsFreeze = true;
        _player.SetState(Player.PlayerState.Dash);
        _animator.SetTrigger("IsDash");
        _player.gameObject.layer = 11; // ���� ���̾�� �ű�

        yield return new WaitForSeconds(0.5f);
        _player.IsStop = false;
        _player.IsFreeze = false;
        _player.gameObject.layer = layer; // ���̾� �ٽ� �ű�

        // ��� ���� ���·� �ǵ�����
        if (_player.IsBattle)
            _player.SetState(Player.PlayerState.Battle);
        else if (_player.IsZoom)
            _player.SetState(Player.PlayerState.Zoom);
        else
            _player.SetState(Player.PlayerState.Idle);
    }
    
    /// <summary>
    /// ��� �����ϰ� �ٲ��ִ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator DashCoroutine()
    {
        _dashAble = false;
        yield return new WaitForSeconds(_coolTime);
        _dashAble = true;
    }
}
