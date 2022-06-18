using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerZoomShoot : MonoBehaviour
{
    // 줌 샷과 관련된 함수를 짜면 됩니당.

    [SerializeField]
    private GameObject _bullet = null;
    [SerializeField]
    private Transform _shootBulletPosition;
    [SerializeField]
    private CinemachineFreeLook _virtualCamera; // 평상시의 버츄얼 카메라


    [SerializeField]
    private float _aimRange = 100f; // 에임을 할 수 있는 사정거리
    [SerializeField]
    private float _zoomAttackDelay = 1f;
    [SerializeField]
    private LayerMask _aimColliderLayerMask = new LayerMask(); // 에임이 딱 갈 수 있는 레이어
    [SerializeField]
    private Transform _debugTrasnform = null; // 에임위치 디버그용

    private bool _isZoomAttacking = false;

    private Player _player = null; // 플레이어

    Vector3 _mouseWorldPosition = Vector3.zero;


    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    public void ZoomIn()
    {
        // 카메라 바꾸기


    }

    public void ZoomOut()
    {
        // 카메라 바꾸기
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
        Debug.Log("줌 불가");
        _isZoomAttacking = true;
        yield return new WaitForSeconds(_zoomAttackDelay);
        _player.IsZoomAttackAble = true;
        _isZoomAttacking = false;
        Debug.Log("줌 ㄱㄴ");
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
