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
    private Transform target;


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

    private Coroutine IsBattleCo = null; // IsBattleCoroutine을 담아둘 Coroutine 변스

    public UnityEvent OnBattle = null; // Battle 상태일 때 실행될 이벤트
    public UnityEvent OnIdle = null; // Battle 상태가 아닐 때 실행될 이벤트
    public UnityEvent OnZoom = null; // 우클릭을 눌렀을 때 실행될 이벤트

    private bool _isStop = false; // true면 안움직임
    public bool IsStop { get => _isStop; set => _isStop = value; } // 인터페이스 구현
    private bool _isBattle = false; // true면 전투상태
    public bool IsBattle { get => _isBattle; set => _isBattle = value; } // _isBattle에 대한 get set 프로퍼티

    private CharacterController _characterController = null; // 캐릭터 컨트롤러 캐싱 준비
    private CinemachineFreeLook cinemacine = null; // 점호
    private CollisionFlags _collisionFlags = CollisionFlags.None; // CollisionFlags 초기화
    private Transform cam = null; // Camera.main.Transform 캐싱 준비
    private Animator _animator = null; // 애니메이터 캐싱 준비

    private PlayerState _playerState = PlayerState.None;

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
        OnIdle?.Invoke();
    }

    private void Update()
    {
        Move();

        Debug.DrawRay(target.position, target.transform.forward * 5f, Color.red);
    }

    private void LateUpdate()
    {
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
            BattleBodyRotate();
    }

    private void BodyRotate(Vector3 dir)
    {
        if (_isStop == true)
            return;

        // 몸통 돌리기
        dir.y = 0f;
        if(dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), _bodyRotateSpeed * Time.deltaTime);
    }

    private void BattleBodyRotate()
    {
        // 배틀 모드일 때 돌리는 코드
        // (머리 레이어만)  animation.Setfloat로 넣어
        // FreeLookCamera의 X axis. Value

    }


    public void OnBattleReset()
    {
        _playerState = PlayerState.Battle;

        transform.rotation = Quaternion.identity;

        CrossHairEnable(true);

        if (IsBattleCo != null)
            StopCoroutine(IsBattleCo);
        IsBattleCo = StartCoroutine(IsBattleCoroutine(_battleDuration));
    }

    public void OnIdleReset()
    {
        _playerState = PlayerState.Idle;

        CrossHairEnable(false);
    }

    public void OnZoomReset()
    {
        OnBattleReset();

        _playerState = PlayerState.Zoom;
    }

    private void CrossHairEnable(bool val)
    {
        _crossHair.enabled = val;
        Image[] crossHairsChildren = null;
        crossHairsChildren = _crossHair.GetComponentsInChildren<Image>();
        foreach (Image e in crossHairsChildren)
        {
            e.enabled = val;
        }
    }

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
