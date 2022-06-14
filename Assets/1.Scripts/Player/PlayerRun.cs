using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    private Player _player = null;
    private Animator _animator = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _player.OnIdle?.Invoke();
            _player.ExitZoom();
            _player.SetState(Player.PlayerState.Run);
            _player.IsRun = true;
            _animator.SetBool("IsRun", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _player.SetState(Player.PlayerState.Idle);
            _player.IsRun = false;
            _animator.SetBool("IsRun", false);
        }
    }
}
