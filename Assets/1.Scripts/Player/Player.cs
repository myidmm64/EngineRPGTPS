using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Define;

public class Player : MonoBehaviour, IMoveAble
{
    [SerializeField]
    private float _speed = 5f; // ĳ������ �̵��ӵ�
    [SerializeField]
    private float _runSpeed = 10f;
    [SerializeField]
    private Vector3 _gravityScale = new Vector3(0f, 9.8f, 0f); // ���߿� ������ �� �߷°�
    [SerializeField]
    private float _bodyRotateSpeed = 10f; // �� ������ �ӵ�
    [SerializeField]
    private float _battleDuration = 3f;
    [SerializeField]
    private float _zoomDuration = 3f;
    [SerializeField]
    private Image _crossHair = null; // ũ�ν����
    private Vector3 amount = Vector3.zero; // �̵��� ���� ���Ͱ�
    private float horizontal = 0f; // Input.GetAxisRaw("Horizontal")
    private float vertical = 0f; // Input.GetAxisRaw("Vertical")
    [SerializeField]
    private BoxCollider _atkCollider = null;
    [SerializeField]
    private GameObject _sword = null;

    private Coroutine IsBattleCo = null; // IsBattleCoroutine�� ��Ƶ� Coroutine ����
    private Coroutine IsZoomCo = null; // IsZoomCoroutine�� ��Ƶ� Coroutine ����

    public UnityEvent OnBattle = null; // Battle ������ �� ����� �̺�Ʈ
    public UnityEvent OnIdle = null; // Battle ���°� �ƴ� �� ����� �̺�Ʈ
    public UnityEvent OnZoom = null; // ��Ŭ���� ������ �� ����� �̺�Ʈ
    public UnityEvent OnZoomShoot = null; // ��Ŭ���� ������ ��Ŭ���� ������ �� ����� �̺�Ʈ
    public UnityEvent OnDash = null; // ��� Ű�� ������ �� ����� �̺�Ʈ

    private bool _isStop = false; // true�� �ȿ�����
    public bool IsStop { get => _isStop; set => _isStop = value; } // �������̽� ����
    private bool _isBattle = false; // true�� ��������
    public bool IsBattle { get => _isBattle; set => _isBattle = value; } // _isBattle�� ���� get set ������Ƽ
    private bool _isZoom = false;
    public bool IsZoom { get => _isZoom; set => _isZoom = value; }
    private bool _isRun = false;
    public bool IsRun { get => _isRun; set => _isRun = value; }
    private bool _isFreeze = false;
    public bool IsFreeze { get => _isFreeze; set => _isFreeze = value; }
    private bool _isAttackAble = true;
    public bool IsAttackAble { get => _isAttackAble; set => _isAttackAble = value; }

    [SerializeField]
    private CinemachineFreeLook _cinemacine = null; // ���� �����ؼ� ������� ����
    [SerializeField]
    private CinemachineFollowZoom _zoom = null; // �� ���� ������� ����

    private CharacterController _characterController = null; // ĳ���� ��Ʈ�ѷ� ĳ�� �غ�
    private CollisionFlags _collisionFlags = CollisionFlags.None; // CollisionFlags �ʱ�ȭ
    private Transform cam = null; // Camera.main.Transform ĳ�� �غ�
    private Animator _animator = null; // �ִϸ����� ĳ�� �غ�

    [SerializeField]
    private PlayerState _playerState = PlayerState.None; // �÷��̾� ����

    public int Coin = 0; // ���� �������ִ� ����

    public enum PlayerState
    {
        None = -1,
        Idle,
        Battle,
        Zoom,
        Run,
        Dash,
        ZoomShot
    }

    private void Awake()
    {
        // ĳ��
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        cam = MainCam.transform;

        //���߿� Core�� �ű��
        Cursor.visible = false;

    }

    private void Start()
    {
        // �ʱ�ȭ
        OnIdle?.Invoke();
        ExitZoom();
    }

    private void Update()
    {
        Move();
        StateDoSomething();
    }

    /// <summary>
    /// ������Ʈ�� ���� ������ ��
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
                BodyRotate(amount);
                break;
            case PlayerState.Battle:
                HeadRotate();
                break;
            case PlayerState.Zoom:
                HeadRotate();
                break;
            case PlayerState.Run:
                BodyRotate(amount);
                break;
            case PlayerState.Dash:
                break;
            case PlayerState.ZoomShot:
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
        GUI.Label(new Rect(10, 60, 100, 200), $"Coin : {Coin}", gUI);


        //if (GUI.Button(new Rect(10,100,600,80), $"ĳ���Ϳ� ���� ���� �ϴ� ��ư", gUI))
        //{
        //    Debug.Log("��ư ���� !!");
        //}
    }

    /// <summary>
    /// ĳ���� �̵� �Լ�
    /// </summary>
    public void Move()
    {
        if (IsFreeze || IsStop)
            return;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // isStop�̰ų� Ű�� ������ �ʾ����� return
        if((horizontal == 0f && vertical == 0f) || _isStop == true)
        {
            _animator.SetBool("IsMove", false);
            return;
        }
        _animator.SetBool("IsMove", true);

        SetMoveAnimation();

        //�̵� ������
        Vector3 forward = cam.TransformDirection(Vector3.forward);
        forward.y = 0f;
        Vector3 right = new Vector3(forward.z , 0f , -forward.x);
        amount = (forward * vertical + right * horizontal) * Time.deltaTime;
        amount *= IsRun ? _runSpeed : _speed;

        if (_collisionFlags == CollisionFlags.None)
            amount -= _gravityScale * Time.deltaTime;

        _collisionFlags = _characterController.Move(amount);

    }

    private void SetMoveAnimation()
    {
        float ver = Input.GetAxis("Vertical");
        float ho = Input.GetAxis("Horizontal");

        _animator.SetFloat("MoveX", ho);
        _animator.SetFloat("MoveY", ver);
    }

    /// <summary>
    /// Battle ���°� �ƴ� �� �� ������ �Լ�
    /// </summary>
    /// <param name="dir"></param>
    private void BodyRotate(Vector3 dir)
    {
        if (IsFreeze)
            return;

        // ���� ������
        dir.y = 0f;
        if(dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), _bodyRotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Battle ������ �� �� ������ �Լ�
    /// </summary>
    private void HeadRotate()
    {

        Quaternion rot = Quaternion.identity;
        //rot = Quaternion.Euler(new Vector3(0f, 60f, 0f));
        rot *= Quaternion.Euler(new Vector3(0f, _cinemacine.m_XAxis.Value, 0f));

        transform.rotation = rot;
    }

    public void SwordAttackStart()
    {
        //attackAble
        _atkCollider.enabled = true;
        IsFreeze = true;
        IsAttackAble = false;
    }

    public void SwordAttackEnd()
    {
        _atkCollider.enabled = false;
        IsFreeze = false;
        IsAttackAble = true;
    }

    /// <summary>
    /// ��Ʋ �̺�Ʈ
    /// </summary>
    public void OnBattleReset()
    {
        _sword.SetActive(true);
        _playerState = PlayerState.Battle;

        CrossHairEnable(false);


        HeadRotate();

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
    /// Idle �̺�Ʈ
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
    /// �� �̺�Ʈ
    /// </summary>
    public void OnZoomReset()
    {
        //��Ʋ ��� �� �����־�� ��
        _sword.SetActive(false);
        _isBattle = false;
        //�ִϸ��̼�

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
    /// �� �� �̺�Ʈ
    /// </summary>
    public void OnZoomShootReset()
    {
        if (IsZoomCo != null)
            StopCoroutine(IsZoomCo);
        IsZoomCo = StartCoroutine(IsZoomCoroutine(_zoomDuration));

        _playerState = PlayerState.ZoomShot;

        Debug.Log("Shoot !!");
    }

    /// <summary>
    /// ���� �ϴ� �Լ�
    /// </summary>
    public void Zoom()
    {
        _playerState = PlayerState.Zoom;
        // ���� �� ������ �ٲٱ�
        _zoom.enabled = true;
        _zoom.m_Width = 5f;

        // LookAt�̶� Follow ������ ����� �ٲ��ֱ�
    }

    /// <summary>
    /// �� �ƿ� �Լ�
    /// </summary>
    public void ExitZoom()
    {
        if (IsZoomCo != null)
            StopCoroutine(IsZoomCo);

        if (IsBattle) // ��Ʋ ���¿��ٸ� ��Ʋ��
        {
            _playerState = PlayerState.Battle;
        }
        else // ���̵� ���¿��ٸ� ���̵��
        {
            IsBattle = false;
            IsZoom = false;
            _playerState = PlayerState.Idle;
        }

        _zoom.enabled = false;

        // LookAt�̶� Follow �ٽ� ������� �ٲ��ֱ�
    }


    //�ӽ� �ڵ�
    public void Ani()
    {
        _animator.SetTrigger("Shoot");
    }

    /// <summary>
    /// ������ ��Ƽ��
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
    /// ��Ʋ ���� ��ȯ
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
    /// �� ���� ��ȯ
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

    public void SetState(PlayerState state)
    {
        _playerState = state;
    }
}
