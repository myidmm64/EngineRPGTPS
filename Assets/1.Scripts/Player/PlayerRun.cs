using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    private Player _player = null; // 플레이어 캐싱 준비
    private PlayerZoomShoot _playerZoomShot = null;
    private Animator _animator = null; // 애니메이터 캐싱 준비

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
        _playerZoomShot = GetComponent<PlayerZoomShoot>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) // 왼쪽 시프트를 누르고있을 때 달리기
        {
            _player.OnIdle?.Invoke();
            _player.SetState(Player.PlayerState.Run);
            _player.IsRun = true;
            _animator.SetBool("IsRun", true);
            _player.ExitZoom();
            _playerZoomShot.ZoomOut();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _player.SetState(Player.PlayerState.Idle);
            _player.IsRun = false;
            _animator.SetBool("IsRun", false);
        }
    }
}
