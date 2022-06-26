using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//불렛이 가지게 될 스크립트
public class BulletProjectile : MonoBehaviour
{
    [SerializeField]
    private GameObject _vfxHitGreen; // 타겟에 맞았을 때 나올 이펙트
    [SerializeField]
    private GameObject _vfxHitRed; // 타겟에 맞지 않았을 때 나올 이펙트
    [SerializeField]
    private float _speed = 40f; // 불렛의 속도
    private Rigidbody _rigid; // 리지드바디 캐싱 준비

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigid.velocity = transform.forward * _speed; //불렛 움직이기
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.GetComponent<SkulMove>() != null) // 만약 스컬에 부딫혔으면 데미지
            other.GetComponent<SkulMove>().Damage(35);

        if (other.GetComponent<BulletTarget>() != null) // 타겟에 맞았는가?
        {
            Instantiate(_vfxHitGreen, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_vfxHitRed, transform.position, Quaternion.identity);
        }
        Destroy(gameObject); // 불렛 파괴
    }
}
