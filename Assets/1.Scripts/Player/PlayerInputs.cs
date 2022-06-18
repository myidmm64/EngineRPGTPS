using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputs : MonoBehaviour
{
    private Player _playerMove = null;

    [field:SerializeField]
    private UnityEvent OnEscapeButton = null;
    [field: SerializeField]
    private UnityEvent OnTapButton = null;

    [SerializeField]
    private GameObject _option = null;
    [SerializeField]
    private GameObject _market = null;

    private bool _isOpenUI = false;
    private bool _isMarketOpen = false;

    private void Awake()
    {
        _playerMove = GetComponent<Player>();
    }

    private void Start()
    {
        _option.SetActive(false);
        _market.SetActive(false);
    }

    private void Update()
    {
        BattleFunc();

        VisibleMenu();

        VisibleMarket();
    }

    private void VisibleMarket()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (_isMarketOpen) // 닫기
            {
                _isMarketOpen = false;
                _market.SetActive(false);
                Cursor.visible = false;
                Time.timeScale = 1f;
            }
            else // 열기
            {
                _isMarketOpen = true;
                _market.SetActive(true);
                Cursor.visible = true;
                Time.timeScale = 0f;
            }
        }
    }

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

    private void BattleFunc()
    {
        //왼쪽 클릭 시 배틀 이벤트
        if(Input.GetMouseButtonDown(0))
        {
            if (_playerMove.IsRun || _playerMove.IsFreeze || _playerMove.IsAttackAble == false) return;

            if (_playerMove.IsZoom == false) // 줌을 안했으면 그냥 근접공격
            {
                _playerMove.IsAttackAble = false;
                _playerMove.OnBattle?.Invoke();
            }
            else if (_playerMove.IsZoom == true) // 만약 줌을 하고있었다면 줌샷 
                _playerMove.OnZoomShoot?.Invoke();
        }
        //오른쪽 클릭 시 줌 이벤트
        else if(Input.GetMouseButtonDown(1))
        {
            if (_playerMove.IsRun || _playerMove.IsFreeze) return;

            _playerMove.OnZoom?.Invoke();
        }
        //오른쪽 클릭 해제 시 줌 해제
        if(Input.GetMouseButtonUp(1))
        {
            if (_playerMove.IsRun || _playerMove.IsFreeze) return;

            _playerMove.ExitZoom();
            _playerMove.CrossHairEnable(false);
            _playerMove.OnZoomOut?.Invoke();
        }
    }

}
