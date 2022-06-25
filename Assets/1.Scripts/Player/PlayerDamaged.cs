using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerDamaged : MonoBehaviour
{
    [SerializeField]
    private int _hp = 10; // �÷��̾��� Hp
    public int HP { get => _hp; set => _hp = value; }
    [SerializeField]
    private float _damageDelay = 1f; // �´� ������

    private bool _isDamage = false; // ���� �°��ִ°�
    private bool _isDead = false; // �׾��°�?
    private Animator _animator = null; // �ִϸ����� ĳ���غ�

    private Player _player = null;

    [field:SerializeField]
    private UnityEvent OnDie = null;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _animator = GetComponent<Animator>();
    }

    private void OnGUI()
    {
        GUIStyle gUI = new GUIStyle();
        gUI.fontSize = 50;
        gUI.fontStyle = FontStyle.Bold;
        gUI.normal.textColor = Color.red;
        GUI.Label(new Rect(10, 60, 100, 200), $"HP : {_hp}", gUI);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAtk")) // ���� ���� ���ݿ� �¾Ҵ°�
        {
            if (_isDamage == false)
            {
                _hp -= other.GetComponent<SkulAttack>().Damage;

                if(_hp <= 0)
                {
                    if (_isDead)
                        return;
                    _isDead = true;

                    OnDie?.Invoke();

                    _animator.SetTrigger("Death");

                    Invoke("Restart", 2f); // �����
                    return;
                }

                StartCoroutine(DamageCoroutine());
            }
        }
    }

    /// <summary>
    /// ���� ����� �Լ�
    /// </summary>
    public void Restart()
    {
        _player.Init();
        SceneManager.LoadScene("SampleScene");
    }

    /// <summary>
    /// �ǰ� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageCoroutine()
    {
        _isDamage = true;
        _animator.SetTrigger("Damage");
        Debug.Log("�¾Ҿ��");
        yield return new WaitForSeconds(_damageDelay);
        _isDamage = false;
    }
}
