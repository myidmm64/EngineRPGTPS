using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMove _playerMove = null;

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        BattleFunc();
    }

    private void BattleFunc()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _playerMove.OnBattle?.Invoke();
            _playerMove.Ani();
        }
        if(Input.GetMouseButtonDown(1))
        {
            _playerMove.OnZoom?.Invoke();
            _playerMove.Ani();
        }
        if(Input.GetMouseButtonUp(1))
        {
            _playerMove.ExitZoom();
        }
    }

}
