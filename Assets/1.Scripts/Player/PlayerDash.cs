using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    //여기에는 대시 관련 코드를 작성하면 됩니당.
    private Animator _animator = null;
    private Rigidbody _rigid = null;
    [SerializeField]
    private float _coolTime = 2f;
    [SerializeField]
    private float _dashPower = 2f;
    private bool _dashAble = true;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(_dashAble)
            {
                _rigid.AddForce(transform.forward * _dashPower, ForceMode.Impulse);
                StartCoroutine(DashAnimation());
                StartCoroutine(Dash());
            }
        }
    }

    private IEnumerator DashAnimation()
    {
        _animator.SetBool("IsDash", true);
        yield return new WaitForSeconds(1f);
        _animator.SetBool("IsDash", false);
    }
    
    private IEnumerator Dash()
    {
        _dashAble = false;
        yield return new WaitForSeconds(_coolTime);
        _dashAble = true;
    }
}
