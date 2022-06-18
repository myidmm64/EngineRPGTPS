using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//코인이 가지게 될 스크립트
public class Coin : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 100f; // 회전속도
    private Vector3 _rotateVec = Vector3.zero; // 회전벡터

    private void Start()
    {
        _rotateVec = new Vector3(0f, 0f, _rotateSpeed * Time.deltaTime); // 회전벡터 캐싱
    }

    private void Update()
    {
        transform.Rotate(_rotateVec); // 회전
    }
}
