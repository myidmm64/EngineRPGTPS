using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputs : MonoBehaviour
{
    private Player _playerMove = null; // �÷��̾� ĳ���غ�
    private PlayerUseSkill _playerUseSkill = null; // �÷��̾� ĳ�� �غ�

    [field:SerializeField]
    private UnityEvent OnEscapeButton = null; // esc ��ư�� ������ �� ����� �̺�Ʈ
    [field: SerializeField]
    private UnityEvent OnTapButton = null; // tab ��ư�� ������ �� ����� �̺�Ʈ

    private Animator _animator = null;

    [SerializeField]
    private GameObject _option = null; // �ɼ�â
    [SerializeField]
    private GameObject _market = null; // ����â

    private bool _isOpenUI = false; // �ɼ� UI�� �����ִ°�?
    private bool _isMarketOpen = false; // ���� UI�� ���� �ִ°�?

    private void Awake()
    {
        _playerMove = GetComponent<Player>();
        _playerUseSkill = GetComponent<PlayerUseSkill>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //�ʱ�ȭ
        _option.SetActive(false);
        _market.SetActive(false);
    }

    private void Update()
    {
        BattleFunc();

        VisibleMenu();

        VisibleMarket();
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    private void VisibleMarket()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (_isMarketOpen) // �ݱ�
            {
                _isMarketOpen = false;
                _market.SetActive(false);
                //Cursor.visible = false;
                Time.timeScale = 1f;
            }
            else // ����
            {
                _isMarketOpen = true;
                _market.SetActive(true);
                //Cursor.visible = true;
                Time.timeScale = 0f;
            }
        }
    }

    /// <summary>
    /// �ɼ� ����
    /// </summary>
    private void VisibleMenu()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_isOpenUI) // �ݱ�
            {
                _isOpenUI = false;
                _option.SetActive(false);
                Time.timeScale = 1f;
            }
            else // ����
            {
                _isOpenUI = true;
                _option.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    /// <summary>
    /// ���� ��ǲ �ޱ�
    /// </summary>
    private void BattleFunc()
    {
        //���� Ŭ�� �� ��Ʋ �̺�Ʈ
        if(Input.GetMouseButtonDown(0))
        {
            if (_playerMove.IsRun || _playerMove.IsFreeze || _playerMove.IsAttackAble == false) return; // ���� �����Ѱ�?

            if (_playerMove.IsZoom == false) // ���� �������� �׳� ��������
            {
                _animator.SetTrigger("Shoot");
                _playerMove.IsAttackAble = false;
                _playerMove.OnBattleReset();
            }
            else if (_playerMove.IsZoom == true && _playerMove.IsZoomAttackAble) // ���� ���� �ϰ��־��ٸ� �ܼ� 
            {
                if (_playerUseSkill.MP <= 0) // ������ ������ ���� ����
                    return;
                _playerUseSkill.MP-=1; // �ܼ��� �� �� ���� ����
                _playerMove.OnZoomShoot?.Invoke();
            }
        }
        //������ Ŭ�� �� �� �̺�Ʈ
        else if(Input.GetMouseButtonDown(1))
        {
            if (_playerMove.IsRun || _playerMove.IsFreeze || _playerMove.IsAttackAble == false) return;

            _playerMove.OnZoom?.Invoke();
        }
        //������ Ŭ�� ���� �� �� ����
        if(Input.GetMouseButtonUp(1))
        {
            if (_playerMove.IsRun || _playerMove.IsFreeze || _playerMove.IsAttackAble == false) return;

            _playerMove.ExitZoom();
            _playerMove.CrossHairEnable(false);
            _playerMove.OnZoomOut?.Invoke();
        }
    }

}
