using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField]
    private GameObject _vfxHitGreen;
    [SerializeField]
    private GameObject _vfxHitRed;
    [SerializeField]
    private float _speed = 40f;
    private Rigidbody _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigid.velocity = transform.forward * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.GetComponent<SkulMove>() != null)
            other.GetComponent<SkulMove>().Damage(10);

        if (other.GetComponent<BulletTarget>() != null)
        {
            Instantiate(_vfxHitGreen, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_vfxHitRed, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
