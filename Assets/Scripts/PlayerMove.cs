using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using static Define;

public class PlayerMove : MonoBehaviour, IMoveAble
{
    [SerializeField]
    private Transform target;


    [SerializeField]
    private float _speed = 5f; // ĳ������ �̵��ӵ�
    [SerializeField]
    private Vector3 _gravityScale = new Vector3(0f, 9.8f, 0f); // ���߿� ������ �� �߷°�
    [SerializeField]
    private float _bodyRotateSpeed = 10f; // �� ������ �ӵ�
    [SerializeField]
    private float _battleDuration = 3f;
    [SerializeField]
    private Image _crossHair = null; // ũ�ν����
    private Vector3 amount = Vector3.zero; // �̵��� ���� ���Ͱ�
    private float horizontal = 0f; // Input.GetAxisRaw("Horizontal")
    private float vertical = 0f; // Input.GetAxisRaw("Vertical")

    private Coroutine IsBattleCo = null; // IsBattleCoroutine�� ��Ƶ� Coroutine ����

    public UnityEvent OnBattle = null; // Battle ������ �� ����� �̺�Ʈ
    public UnityEvent OnIdle = null; // Battle ���°� �ƴ� �� ����� �̺�Ʈ
    public UnityEvent OnZoom = null; // ��Ŭ���� ������ �� ����� �̺�Ʈ

    private bool _isStop = false; // true�� �ȿ�����
    public bool IsStop { get => _isStop; set => _isStop = value; } // �������̽� ����
    private bool _isBattle = false; // true�� ��������
    public bool IsBattle { get => _isBattle; set => _isBattle = value; } // _isBattle�� ���� get set ������Ƽ

    [SerializeField]
    private CinemachineFreeLook _cinemacine = null; // ���� �����ؼ� ������� ����
    [SerializeField]
    private CinemachineFollowZoom _zoom = null; // �� ���� ������� ����

    private CharacterController _characterController = null; // ĳ���� ��Ʈ�ѷ� ĳ�� �غ�
    private CollisionFlags _collisionFlags = CollisionFlags.None; // CollisionFlags �ʱ�ȭ
    private Transform cam = null; // Camera.main.Transform ĳ�� �غ�
    private Animator _animator = null; // �ִϸ����� ĳ�� �غ�

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
        // ĳ��
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        cam = MainCam.transform;

        //���߿� Core�� �ű��
        Cursor.visible = false;

    }

    private void Start()
    {
        OnIdle?.Invoke();
        Zoom();
        ExitZoom();
    }

    private void Update()
    {
        Move();
        //HeadRotate();

        //Debug.DrawRay(target.position, target.transform.forward * 5f, Color.red);
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
        if(GUI.Button(new Rect(10,100,600,80), $"ĳ���Ϳ� ���� ���� �ϴ� ��ư", gUI))
        {
            Debug.Log("��ư ���� !!");
        }
    }

    public void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // isStop�̰ų� Ű�� ������ �ʾ����� return
        if((horizontal == 0f && vertical == 0f) || _isStop == true)
        {
            _animator.SetBool("IsMove", false);
            return;
        }
        _animator.SetBool("IsMove", true);

        //�̵� ������
        Vector3 forward = cam.TransformDirection(Vector3.forward);
        forward.y = 0f;
        Vector3 right = new Vector3(forward.z , 0f , -forward.x);
        amount = (forward * vertical + right * horizontal) * _speed * Time.deltaTime;

        if (_collisionFlags == CollisionFlags.None)
            amount -= _gravityScale * Time.deltaTime;

        _collisionFlags = _characterController.Move(amount);

        //���� ������ �Լ�
        if (_isBattle == false)
            BodyRotate(amount);
    }

    private void BodyRotate(Vector3 dir)
    {
        if (_isStop == true)
            return;

        // ���� ������
        dir.y = 0f;
        if(dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), _bodyRotateSpeed * Time.deltaTime);
    }

    private void HeadRotate()
    {
        switch(_playerState)
        {
            case PlayerState.None:
                return;
            case PlayerState.Idle:
                return;
        }
        Quaternion rot;
        rot = Quaternion.Euler(new Vector3(0f, 60f, 0f));
        rot *= Quaternion.Euler(new Vector3(0f, _cinemacine.m_XAxis.Value, 0f));

        Debug.Log(_cinemacine.m_XAxis.Value);
        transform.rotation = rot;
    }


    public void OnBattleReset()
    {
        _playerState = PlayerState.Battle;

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

        Zoom();

        _playerState = PlayerState.Zoom;
    }

    public void Zoom()
    {
        // ���� �� ������ �ٲٱ�
        _zoom.enabled = true;
        _zoom.m_Width = 5f;
    }

    public void Ani()
    {
        _animator.SetTrigger("Shoot");
    }

    public void ExitZoom()
    {
        _zoom.enabled = false;
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
