using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    private PlayerMove _playerMove = null;

    [field:SerializeField]
    private UnityEvent OnEscapeButton = null;

    [SerializeField]
    private GameObject _option = null;
    private bool _isOpenUI = false;

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
    }

    private void Start()
    {
        _option.SetActive(false);
    }

    private void Update()
    {
        BattleFunc();

        VisibleMenu();
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
            if (_playerMove.IsZoom == false)
                _playerMove.OnBattle?.Invoke();
            else if (_playerMove.IsZoom == true)
                _playerMove.OnZoomShoot?.Invoke();
        }
        //오른쪽 클릭 시 줌 이벤트
        else if(Input.GetMouseButtonDown(1))
        {
            _playerMove.OnZoom?.Invoke();
        }
        //오른쪽 클릭 해제 시 줌 해제
        if(Input.GetMouseButtonUp(1))
        {
            _playerMove.ExitZoom();
            _playerMove.CrossHairEnable(false);
        }
    }

}
