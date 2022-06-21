using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputs : MonoBehaviour
{
    private Player _playerMove = null; // 플레이어 캐싱준비
    private PlayerUseSkill _playerUseSkill = null; // 플레이어 캐싱 준비

    [field:SerializeField]
    private UnityEvent OnEscapeButton = null; // esc 버튼이 눌렸을 때 발행될 이벤트
    [field: SerializeField]
    private UnityEvent OnTapButton = null; // tab 버튼이 눌렸을 때 발행될 이벤트

    private Animator _animator = null;

    [SerializeField]
    private GameObject _option = null; // 옵션창
    [SerializeField]
    private GameObject _market = null; // 상점창

    private bool _isOpenUI = false; // 옵션 UI를 열고있는가?
    private bool _isMarketOpen = false; // 상점 UI를 열고 있는가?

    private void Awake()
    {
        _playerMove = GetComponent<Player>();
        _playerUseSkill = GetComponent<PlayerUseSkill>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //초기화
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
    /// 상점 띄우기
    /// </summary>
    private void VisibleMarket()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (_isMarketOpen) // 닫기
            {
                _isMarketOpen = false;
                _market.SetActive(false);
                //Cursor.visible = false;
                Time.timeScale = 1f;
            }
            else // 열기
            {
                _isMarketOpen = true;
                _market.SetActive(true);
                //Cursor.visible = true;
                Time.timeScale = 0f;
            }
        }
    }

    /// <summary>
    /// 옵션 띄우기
    /// </summary>
    private void VisibleMenu()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_isOpenUI) // 닫기
            {
                _isOpenUI = false;
                _option.SetActive(false);
                Time.timeScale = 1f;
            }
            else // 열기
            {
                _isOpenUI = true;
                _option.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    /// <summary>
    /// 전투 인풋 받기
    /// </summary>
    private void BattleFunc()
    {
        //왼쪽 클릭 시 배틀 이벤트
        if(Input.GetMouseButtonDown(0))
        {
            if (_playerMove.IsRun || _playerMove.IsFreeze || _playerMove.IsAttackAble == false) return; // 공격 가능한가?

            if (_playerMove.IsZoom == false) // 줌을 안했으면 그냥 근접공격
            {
                _animator.SetTrigger("Shoot");
                _playerMove.IsAttackAble = false;
                _playerMove.OnBattleReset();
            }
            else if (_playerMove.IsZoom == true && _playerMove.IsZoomAttackAble) // 만약 줌을 하고있었다면 줌샷 
            {
                if (_playerUseSkill.MP <= 0) // 마나가 없으면 실행 안함
                    return;
                _playerUseSkill.MP-=1; // 줌샷을 할 때 마나 감소
                _playerMove.OnZoomShoot?.Invoke();
            }
        }
        //오른쪽 클릭 시 줌 이벤트
        else if(Input.GetMouseButtonDown(1))
        {
            if (_playerMove.IsRun || _playerMove.IsFreeze || _playerMove.IsAttackAble == false) return;

            _playerMove.OnZoom?.Invoke();
        }
        //오른쪽 클릭 해제 시 줌 해제
        if(Input.GetMouseButtonUp(1))
        {
            if (_playerMove.IsRun || _playerMove.IsFreeze || _playerMove.IsAttackAble == false) return;

            _playerMove.ExitZoom();
            _playerMove.CrossHairEnable(false);
            _playerMove.OnZoomOut?.Invoke();
        }
    }

}
