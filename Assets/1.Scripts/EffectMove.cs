using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMove : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

    private void Update()
    {
        transform.Translate(Quaternion.Euler(new Vector3(0, -90f, 0)) * transform.forward.normalized * _speed * Time.deltaTime, Space.World);
    }
}
