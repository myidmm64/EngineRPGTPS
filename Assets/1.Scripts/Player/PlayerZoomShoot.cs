using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerZoomShoot : MonoBehaviour
{
    // �� ���� ���õ� �Լ��� ¥�� �˴ϴ�.

    [SerializeField]
    private GameObject _bullet = null;
    [SerializeField]
    private Transform _shootBulletPosition;
    [SerializeField]
    private CinemachineFreeLook _virtualCamera; // ������ ����� ī�޶�


    [SerializeField]
    private float _aimRange = 100f; // ������ �� �� �ִ� �����Ÿ�
    [SerializeField]
    private float _zoomAttackDelay = 1f;
    [SerializeField]
    private LayerMask _aimColliderLayerMask = new LayerMask(); // ������ �� �� �� �ִ� ���̾�
    [SerializeField]
    private Transform _debugTrasnform = null; // ������ġ ����׿�

    private bool _isZoomAttacking = false;

    private Player _player = null; // �÷��̾�

    Vector3 _mouseWorldPosition = Vector3.zero;


    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    public void ZoomIn()
    {
        // ī�޶� �ٲٱ�


    }

    public void ZoomOut()
    {
        // ī�޶� �ٲٱ�
    }

    public void ZoomShootDelay()
    {
        if (_isZoomAttacking)
            return;

        StartCoroutine(ZoomShootDelayCoroutine());
    }

    private IEnumerator ZoomShootDelayCoroutine()
    {
        _player.IsZoomAttackAble = false;
        Debug.Log("�� �Ұ�");
        _isZoomAttacking = true;
        yield return new WaitForSeconds(_zoomAttackDelay);
        _player.IsZoomAttackAble = true;
        _isZoomAttacking = false;
        Debug.Log("�� ����");
    }


    private void Update()
    {
        if (_player.IsZoom == false)
            return;

        _mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = MainCam.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, _aimRange, _aimColliderLayerMask))
        {
            Debug.Log(raycastHit.collider.gameObject.name);
            _debugTrasnform.position = raycastHit.point;
            _mouseWorldPosition = raycastHit.point;
        }

        Vector3 worldAimTartget = _mouseWorldPosition;
        worldAimTartget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTartget - transform.position).normalized;
    }

    public void ShootBullet()
    {
        Vector3 aimDir = (_mouseWorldPosition - _shootBulletPosition.position).normalized;

        Instantiate(_bullet, _shootBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
    }
}
