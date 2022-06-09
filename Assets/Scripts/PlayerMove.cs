using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Define;

public class PlayerMove : MonoBehaviour, IMoveAble
{
    [SerializeField]
    private float _speed = 5f; // 캐릭터의 이동속도
    [SerializeField]
    private Vector3 _gravityScale = new Vector3(0f, 9.8f, 0f); // 공중에 떠있을 때 중력값
    [SerializeField]
    private float _bodyRotateSpeed = 10f; // 몸 돌리는 속도
    [SerializeField]
    private float _battleDuration = 3f;
    [SerializeField]
    private Image _crossHair = null; // 크로스헤어
    private Vector3 amount = Vector3.zero; // 이동에 대한 벡터값
    private float horizontal = 0f; // Input.GetAxisRaw("Horizontal")
    private float vertical = 0f; // Input.GetAxisRaw("Vertical")
    [SerializeField]
    private BoxCollider _atkCollider = null;

    private Coroutine IsBattleCo = null; // IsBattleCoroutine을 담아둘 Coroutine 변스

    public UnityEvent OnBattle = null; // Battle 상태일 때 실행될 이벤트
    public UnityEvent OnIdle = null; // Battle 상태가 아닐 때 실행될 이벤트
    public UnityEvent OnZoom = null; // 우클릭을 눌렀을 때 실행될 이벤트

    private bool _isStop = false; // true면 안움직임
    public bool IsStop { get => _isStop; set => _isStop = value; } // 인터페이스 구현
    private bool _isBattle = false; // true면 전투상태
    public bool IsBattle { get => _isBattle; set => _isBattle = value; } // _isBattle에 대한 get set 프로퍼티

    [SerializeField]
    private CinemachineFreeLook _cinemacine = null; // 감도 관련해서 갖고오기 위함
    [SerializeField]
    private CinemachineFollowZoom _zoom = null; // 줌 관련 갖고오기 위함

    private CharacterController _characterController = null; // 캐릭터 컨트롤러 캐싱 준비
    private CollisionFlags _collisionFlags = CollisionFlags.None; // CollisionFlags 초기화
    private Transform cam = null; // Camera.main.Transform 캐싱 준비
    private Animator _animator = null; // 애니메이터 캐싱 준비

    [SerializeField]
    private PlayerState _playerState = PlayerState.None; // 플레이어 상태

    public enum PlayerState
    {
        None = -1,
        Idle,
        Battle,
        Zoom
    }

    private void Awake()
    {
        // 캐싱
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        cam = MainCam.transform;

        //나중에 Core로 옮기셈
        Cursor.visible = false;

    }

    private void Start()
    {
        // 초기화
        OnIdle?.Invoke();
        ExitZoom();
    }

    private void Update()
    {
        Move();
        StateDoSomething();
    }

    /// <summary>
    /// 스테이트에 따라 실행할 것
    /// </summary>
    private void StateDoSomething()
    {
        switch(_playerState)
        {
            case PlayerState.None:
                break;
            case PlayerState.Idle:
                break;
            case PlayerState.Battle:
                break;
            case PlayerState.Zoom:
                break;
            default:
                break;
        }
    }

    private void OnGUI()
    {
        GUIStyle gUI = new GUIStyle();
        gUI.fontSize = 50;
        gUI.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect(10, 10, 100 , 200), $"CollisionFlags : {_collisionFlags}", gUI);
        if(GUI.Button(new Rect(10,100,600,80), $"캐릭터에 대해 무언가 하는 버튼", gUI))
        {
            Debug.Log("버튼 누름 !!");
        }
    }

    /// <summary>
    /// 캐릭터 이동 함수
    /// </summary>
    public void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // isStop이거나 키가 눌리지 않았으면 return
        if((horizontal == 0f && vertical == 0f) || _isStop == true)
        {
            _animator.SetBool("IsMove", false);
            return;
        }
        _animator.SetBool("IsMove", true);

        //이동 구현부
        Vector3 forward = cam.TransformDirection(Vector3.forward);
        forward.y = 0f;
        Vector3 right = new Vector3(forward.z , 0f , -forward.x);
        amount = (forward * vertical + right * horizontal) * _speed * Time.deltaTime;

        if (_collisionFlags == CollisionFlags.None)
            amount -= _gravityScale * Time.deltaTime;

        _collisionFlags = _characterController.Move(amount);

        //몸을 돌리는 함수
        if (_isBattle == false)
            BodyRotate(amount);
        else
            HeadRotate();
    }

    /// <summary>
    /// Battle 상태가 아닐 때 몸 돌리는 함수
    /// </summary>
    /// <param name="dir"></param>
    private void BodyRotate(Vector3 dir)
    {
        if (_isStop == true)
            return;

        // 몸통 돌리기
        dir.y = 0f;
        if(dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), _bodyRotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Battle 상태일 때 몸 돌리는 함수
    /// </summary>
    private void HeadRotate()
    {
        switch(_playerState)
        {
            case PlayerState.None:
                return;
            case PlayerState.Idle:
                return;
        }
        Quaternion rot = Quaternion.identity;
        //rot = Quaternion.Euler(new Vector3(0f, 60f, 0f));
        rot *= Quaternion.Euler(new Vector3(0f, _cinemacine.m_XAxis.Value, 0f));

        Debug.Log(_cinemacine.m_XAxis.Value);
        transform.rotation = rot;
    }

    /// <summary>
    /// 배틀 이벤트
    /// </summary>
    public void OnBattleReset()
    {
        _playerState = PlayerState.Battle;

        CrossHairEnable(false);

        _atkCollider.enabled = true;
        Invoke("OffCollider", 0.5f);

        if (IsBattleCo != null)
            StopCoroutine(IsBattleCo);
        IsBattleCo = StartCoroutine(IsBattleCoroutine(_battleDuration));
    }

    private void OffCollider()
    {
        _atkCollider.enabled = false;
    }

    /// <summary>
    /// Idle 이벤트
    /// </summary>
    public void OnIdleReset()
    {
        IsBattle = false;

        _playerState = PlayerState.Idle;

        CrossHairEnable(false);

        if (IsBattleCo != null)
            StopCoroutine(IsBattleCo);
    }

    /// <summary>
    /// 줌 이벤트
    /// </summary>
    public void OnZoomReset()
    {

        CrossHairEnable(true);

        Zoom();

        //밑 코드 수정 필요.
        if (IsBattleCo != null)
            StopCoroutine(IsBattleCo);
        IsBattleCo = StartCoroutine(IsBattleCoroutine(_battleDuration));
    }

    /// <summary>
    /// 줌인 하는 함수
    /// </summary>
    public void Zoom()
    {
        _playerState = PlayerState.Zoom;
        // 감도 줌 감도로 바꾸기
        _zoom.enabled = true;
        _zoom.m_Width = 5f;
    }

    /// <summary>
    /// 줌 아웃 함수
    /// </summary>
    public void ExitZoom()
    {
        if(IsBattle)
        {
            OnBattle?.Invoke();
        }
        else
        {
            OnIdle?.Invoke();
        }

        _zoom.enabled = false;
    }


    //임시 코드
    public void Ani()
    {
        _animator.SetTrigger("Shoot");
    }

    /// <summary>
    /// 조준점 액티브
    /// </summary>
    /// <param name="val"></param>
    public void CrossHairEnable(bool val)
    {
        _crossHair.enabled = val;
        Image[] crossHairsChildren = null;
        crossHairsChildren = _crossHair.GetComponentsInChildren<Image>();
        foreach (Image e in crossHairsChildren)
        {
            e.enabled = val;
        }
    }

    /// <summary>
    /// 배틀 상태 변환
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator IsBattleCoroutine(float time)
    {
        Debug.Log("IsBattle");
        _isBattle = true;
        _animator.SetBool("IsBattle", _isBattle);
        yield return new WaitForSeconds(time);
        _isBattle = false;
        _animator.SetBool("IsBattle", _isBattle);
        OnIdle?.Invoke();
        Debug.Log("False");
    }
}
