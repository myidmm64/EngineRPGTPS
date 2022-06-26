using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using static Define;

public class Player : MonoBehaviour, IMoveAble
{
    [SerializeField]
    private float _speed = 5f; // 캐릭터의 이동속도
    [SerializeField]
    private int _maxAnimationIdx = 2;
    [SerializeField]
    private float _runSpeed = 10f; // 캐릭터 달리기 이동속도
    [SerializeField]
    private Vector3 _gravityScale = new Vector3(0f, 9.8f, 0f); // 공중에 떠있을 때 중력값
    [SerializeField]
    private float _bodyRotateSpeed = 10f; // 몸 돌리는 속도
    [SerializeField]
    private float _headRotateSpeed = 2f; // 마우스에 따라 몸을 돌리는 속도
    [SerializeField]
    private Vector2 _defaultSensitivity = Vector2.zero;
    [SerializeField]
    private float _normalSensitivity = 1f; // 평상 시 마우스 감도
    [SerializeField]
    private float _aimSensiticity = 0.5f; // 줌 시 마우스 감도
    [SerializeField]
    private float _battleDuration = 3f; // 전투 지속시간
    [SerializeField]
    private float _zoomDuration = 3f; // 줌 지속시간
    [SerializeField]
    private Image _crossHair = null; // 크로스헤어
    private Vector3 _amount = Vector3.zero; // 이동에 대한 벡터값
    private float _horizontal = 0f; // Input.GetAxisRaw("Horizontal")
    private float _vertical = 0f; // Input.GetAxisRaw("Vertical")
    [SerializeField]
    private BoxCollider _atkCollider = null; // 플레이어 공격 콜라이더
    [SerializeField]
    private GameObject _sword = null; // 검 오브젝트
    private PlayerAttack _playerAttack = null;

    private Coroutine IsBattleCo = null; // IsBattleCoroutine을 담아둘 Coroutine 변수
    private Coroutine IsZoomCo = null; // IsZoomCoroutine을 담아둘 Coroutine 변수
    private Coroutine IsAttackCo = null; // 을 담아둘 Coroutine 변수

    private int _attackCnt = 0;
    public int AttackCnt { get => _attackCnt; }

    public UnityEvent OnBattle = null; // Battle 상태일 때 실행될 이벤트
    public UnityEvent OnIdle = null; // Battle 상태가 아닐 때 실행될 이벤트
    public UnityEvent OnZoom = null; // 우클릭을 눌렀을 때 실행될 이벤트
    public UnityEvent OnZoomOut = null; // 
    public UnityEvent OnZoomShoot = null; // 우클릭을 누르고 좌클릭을 눌렀을 때 실행될 이벤트
    public UnityEvent OnDash = null; // 대시 키를 눌렀을 때 실행될 이벤트

    private bool _isStop = false; // true면 안움직임
    public bool IsStop { get => _isStop; set => _isStop = value; } // 인터페이스 구현
    private bool _isBattle = false; // true면 전투상태
    public bool IsBattle { get => _isBattle; set => _isBattle = value; }
    private bool _isZoom = false; // true면 줌 상태
    public bool IsZoom { get => _isZoom; set => _isZoom = value; }
    private bool _isRun = false; // true면 달리기 상태
    public bool IsRun { get => _isRun; set => _isRun = value; }
    private bool _isFreeze = false; // true면 움직이지 못하는 상태
    public bool IsFreeze { get => _isFreeze; set => _isFreeze = value; }
    private bool _isAttackAble = true; // true면 왼쪽클릭 가능
    public bool IsAttackAble { get => _isAttackAble; set => _isAttackAble = value; }
    private bool _isZoomAttackAble = true; // true면 총알 쏘기 가능
    public bool IsZoomAttackAble { get => _isZoomAttackAble; set => _isZoomAttackAble = value; }

    [SerializeField]
    private CinemachineFreeLook _cinemacine = null; // 감도 관련해서 갖고오기 위함
    [SerializeField]
    private CinemachineFollowZoom _zoom = null; // 줌 관련 갖고오기 위함

    private CharacterController _characterController = null; // 캐릭터 컨트롤러 캐싱 준비
    private CollisionFlags _collisionFlags = CollisionFlags.None; // CollisionFlags 초기화
    private Transform _cam = null; // Camera.main.Transform 캐싱 준비
    private Animator _animator = null; // 애니메이터 캐싱 준비

    [SerializeField]
    private PlayerState _playerState = PlayerState.None; // 플레이어 상태
    public PlayerState playerState => _playerState;

    private int _coin = 0;
    public int Coin { get => _coin; set => _coin = value; } // 현재 가지고있는 코인 (private로 바꿔주세요)

    private int _monsterCnt = 0; //
    public int MonsterCnt
    {
        get => _monsterCnt;
        set
        {
            _monsterCnt = value;
            if(_monsterCnt >= 10)
            {
                Debug.Log("ㅁㄴㅇㅁ");
            }
        }
    }

    //플레이어의 상태
    public enum PlayerState
    {
        None = -1,
        Idle,
        Battle,
        Zoom,
        Run,
        Dash,
        ZoomShot,
        Skilling
    }

    private void Awake()
    {
        // 캐싱
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _playerAttack = _atkCollider.GetComponent<PlayerAttack>();
        _cam = MainCam.transform;

        //나중에 Core로 옮기셈
        //Cursor.visible = false; // 마우스 포인터 비활성화

    }

    private void Start()
    {
        // 초기화
        Init();
    }

    public void Init()
    {
        _defaultSensitivity = new Vector2(_cinemacine.m_XAxis.m_MaxSpeed, _cinemacine.m_YAxis.m_MaxSpeed);
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
        if (IsFreeze)
            return;

        switch (_playerState)
        {
            case PlayerState.None:
                break;
            case PlayerState.Idle:
                BodyRotate(_amount);
                break;
            case PlayerState.Battle:
                HeadRotate();
                break;
            case PlayerState.Zoom:
                HeadRotate();
                break;
            case PlayerState.Run:
                BodyRotate(_amount);
                break;
            case PlayerState.Dash:
                break;
            case PlayerState.ZoomShot:
                HeadRotate();
                break;
            case PlayerState.Skilling:
                HeadRotate();
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
        gUI.normal.textColor = Color.red;
        GUI.Label(new Rect(10, 100, 100, 200), $"Coin : {Coin}", gUI);
        GUI.Label(new Rect(10, 260, 100, 200), $"cnt : {MonsterCnt}", gUI);


        //if (GUI.Button(new Rect(10,100,600,80), $"캐릭터에 대해 무언가 하는 버튼", gUI))
        //{
        //    Debug.Log("버튼 누름 !!");
        //}
    }

    /// <summary>
    /// 캐릭터 이동 함수
    /// </summary>
    public void Move()
    {
        if (IsFreeze || IsStop)
            return;

        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        // isStop이거나 키가 눌리지 않았으면 return
        if((_horizontal == 0f && _vertical == 0f) || _isStop == true)
        {
            _animator.SetBool("IsMove", false);
            return;
        }
        _animator.SetBool("IsMove", true);

        SetMoveAnimation();

        //이동 구현부
        Vector3 forward = _cam.TransformDirection(Vector3.forward);
        forward.y = 0f;
        Vector3 right = new Vector3(forward.z , 0f , -forward.x);
        _amount = (forward * _vertical + right * _horizontal) * Time.deltaTime;
        _amount *= IsRun ? _runSpeed : _speed;

        if (_collisionFlags == CollisionFlags.None)
            _amount -= _gravityScale * Time.deltaTime;

        _collisionFlags = _characterController.Move(_amount);

    }

    /// <summary>
    /// 애니메이터에 Setfloat로 넣을 값 설정
    /// </summary>
    private void SetMoveAnimation()
    {
        float ver = Input.GetAxis("Vertical");
        float ho = Input.GetAxis("Horizontal");

        _animator.SetFloat("MoveX", ho);
        _animator.SetFloat("MoveY", ver);
    }

    /// <summary>
    /// Battle 상태가 아닐 때 몸 돌리는 함수
    /// </summary>
    /// <param name="dir"></param>
    private void BodyRotate(Vector3 dir)
    {
        if (IsFreeze)
            return;

        // 몸통 돌리기
        dir.y = 0f;
        if(dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), _bodyRotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 전투 상태일 때 몸 돌리는 함수
    /// </summary>
    /// <param name="즉시 회전"></param>
    public void HeadRotate(bool immediately = false)
    {

        Quaternion rot = Quaternion.identity;
        //rot = Quaternion.Euler(new Vector3(0f, 60f, 0f));
        rot *= Quaternion.Euler(new Vector3(0f, _cinemacine.m_XAxis.Value, 0f));

        if (immediately)
            transform.rotation = rot;
        else
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, _headRotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 어택 애니메이션 시작헀을 때 실행될 것
    /// </summary>
    public void SwordAttackStart()
    {
        //attackAble
        if (IsAttackCo != null)
            StopCoroutine(IsAttackCo);

        IsAttackCo = StartCoroutine(IsAttackCoroutine());

        OnBattle?.Invoke();
        _sword.SetActive(true);
        _atkCollider.enabled = true;
        IsFreeze = true;
        IsAttackAble = false;
    }

    /// <summary>
    /// 연속 공격 코루틴 !!
    /// </summary>
    /// <returns></returns>
    private IEnumerator IsAttackCoroutine()
    {
        _attackCnt++;
        _animator.SetInteger("AttackCnt", _attackCnt);
        Debug.Log($"attack Cnt => {_attackCnt}");

        _playerAttack.AttackEffectSpawn(_attackCnt);

        if (_attackCnt == _maxAnimationIdx + 1)
        {
            _attackCnt = 0;
            _animator.SetInteger("AttackCnt", _attackCnt);
            StopCoroutine(IsAttackCo);
        }

        yield return new WaitForSeconds(1f);
        _attackCnt = 0;
        _animator.SetInteger("AttackCnt", _attackCnt);
        Debug.Log($"어택 카운트 초기화");
    }
    
    /// <summary>
    /// 어택 애니메이션 끝냈을 때 실행될 것
    /// </summary>
    public void SwordAttackEnd()
    {
        _atkCollider.enabled = false;
        IsFreeze = false;
        IsAttackAble = true;
    }

    /// <summary>
    /// 배틀 이벤트
    /// </summary>
    public void OnBattleReset()
    {
        _sword.SetActive(true);
        _playerState = PlayerState.Battle;

        CrossHairEnable(false);

        HeadRotate(true);

        if (IsZoomCo != null)
            StopCoroutine(IsZoomCo);
        if (IsBattleCo != null)
        {
            StopCoroutine(IsBattleCo);
            _isBattle = false;
            _animator.SetBool("IsBattle", _isBattle);
        }
        IsBattleCo = StartCoroutine(IsBattleCoroutine(_battleDuration));
    }


    /// <summary>
    /// Idle 이벤트
    /// </summary>
    public void OnIdleReset()
    {
        _sword.SetActive(false);

        IsBattle = false;
        IsZoom = false;

        _playerState = PlayerState.Idle;

        CrossHairEnable(false);

        if (IsBattleCo != null)
        {
            StopCoroutine(IsBattleCo);
            _isBattle = false;
            _animator.SetBool("IsBattle", _isBattle);
        }
        if (IsZoomCo != null)
            StopCoroutine(IsZoomCo);
    }

    /// <summary>
    /// 줌 이벤트
    /// </summary>
    public void OnZoomReset()
    {
        //배틀 기능 다 멈춰주어야 함
        _sword.SetActive(false);
        _isBattle = false;
        //애니메이션

        CrossHairEnable(true);

        Zoom();

        if (IsBattleCo != null)
        {
            StopCoroutine(IsBattleCo);
            _isBattle = false;
            _animator.SetBool("IsBattle", _isBattle);
        }
        if (IsZoomCo != null)
            StopCoroutine(IsZoomCo);
        IsZoomCo = StartCoroutine(IsZoomCoroutine(_zoomDuration));
    }

    /// <summary>
    /// 줌 샷 이벤트
    /// </summary>
    public void OnZoomShootReset()
    {
        if (IsZoomCo != null)
            StopCoroutine(IsZoomCo);
        IsZoomCo = StartCoroutine(IsZoomCoroutine(_zoomDuration));

        _animator.SetTrigger("ZoomShoot");
        _playerState = PlayerState.ZoomShot;

        Debug.Log("Shoot !!");
    }

    /// <summary>
    /// 줌인 하는 함수
    /// </summary>
    public void Zoom()
    {
        _playerState = PlayerState.Zoom;
        // 감도 줌 감도로 바꾸기
        _zoom.enabled = true;
        _zoom.m_Width = 2f;

        _animator.SetBool("Zoom", true);

        // 감도 줌 감도로 바꾸기
        _cinemacine.m_XAxis.m_MaxSpeed = _defaultSensitivity.x * _aimSensiticity; 
        _cinemacine.m_YAxis.m_MaxSpeed = _defaultSensitivity.y * _aimSensiticity;

    }

    /// <summary>
    /// 줌 아웃 함수
    /// </summary>
    public void ExitZoom()
    {
        if (IsZoomCo != null)
            StopCoroutine(IsZoomCo);

        if (IsBattle) // 배틀 상태였다면 배틀로
        {
            _playerState = PlayerState.Battle;
        }
        else // 아이들 상태였다면 아이들로
        {
            IsBattle = false;
            IsZoom = false;
            _playerState = PlayerState.Idle;
        }

        _animator.SetBool("Zoom", false);

        _zoom.enabled = false;

        //감도 원래대로 돌려놓기
        _cinemacine.m_XAxis.m_MaxSpeed = _defaultSensitivity.x * _normalSensitivity;
        _cinemacine.m_YAxis.m_MaxSpeed = _defaultSensitivity.y * _normalSensitivity;
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
        _isBattle = true;
        _animator.SetBool("IsBattle", _isBattle);
        yield return new WaitForSeconds(time);
        _isBattle = false;
        _animator.SetBool("IsBattle", _isBattle);
        OnIdle?.Invoke();
    }

    /// <summary>
    /// 줌 상태 변환
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator IsZoomCoroutine(float time)
    {
        _isZoom = true;
        _animator.SetBool("IsBattle", _isZoom);
        yield return new WaitForSeconds(time);
        _isZoom = false;
        _animator.SetBool("IsBattle", _isZoom);
        OnIdle?.Invoke();
        ExitZoom();
    }

    /// <summary>
    /// 플레이어 상태 변환
    /// </summary>
    /// <param name="state"></param>
    public void SetState(PlayerState state)
    {
        _playerState = state;
    }

    public void Dead()
    {
        ExitZoom();
        MonoBehaviour[] monoBehaviours = GetComponents<MonoBehaviour>();
        foreach(var m in monoBehaviours)
        {
            m.StopAllCoroutines();
            m.DOKill();
        }

        IsFreeze = true;
        IsStop = true;
        IsAttackAble = false;
        IsZoomAttackAble = false;
    }
}
