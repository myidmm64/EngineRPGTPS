using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    private Player _playerMove = null;
    private Animator _animator = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMove = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _playerMove.OnIdle?.Invoke();
            _playerMove.ExitZoom();
            _playerMove.IsRun = true;
            _animator.SetBool("IsRun", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _playerMove.IsRun = false;
            _animator.SetBool("IsRun", false);
        }
    }
}
