using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDamaged : MonoBehaviour
{
    [SerializeField]
    private int _hp = 10; // 플레이어의 Hp
    public int HP { get => _hp; set => _hp = value; }
    [SerializeField]
    private float _damageDelay = 1f; // 맞는 딜레이

    private bool _isDamage = false; // 현재 맞고있는가
    private Animator _animator = null; // 애니메이터 캐싱준비

    private void Awake()
    {
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
        if (other.CompareTag("EnemyAtk")) // 만약 적의 공격에 맞았는가
        {
            if (_isDamage == false)
            {
                _hp -= 1;
                if(_hp <= 0)
                {
                    _animator.SetTrigger("Death");
                    Invoke("Restart", 2f); // 재시작
                    return;
                }

                StartCoroutine(DamageCoroutine());
            }
        }
    }

    /// <summary>
    /// 게임 재시작 함수
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    /// <summary>
    /// 피격 상태 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageCoroutine()
    {
        _isDamage = true;
        _animator.SetTrigger("Damage");
        Debug.Log("맞았어요");
        yield return new WaitForSeconds(_damageDelay);
        _isDamage = false;
    }
}
