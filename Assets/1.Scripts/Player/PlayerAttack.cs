using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _attackEffect = new List<GameObject>(); // 어택 이펙트 리스트
    [SerializeField]
    private Transform _playerTransform = null; // 플레이어의 트랜스폼
    [SerializeField]
    private int _damage = 10; // 검의 데미지

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy")) 
        {
            other.GetComponent<SkulMove>()?.Damage(damage : _damage); //닿은 녀석에게 데미지 주기
        }
    }

    public void AttackEffectSpawn()
    {
        int random = Random.Range(0, _attackEffect.Count); // 이펙트 인덱스 랜덤으로
        GameObject obj = Instantiate(_attackEffect[random], _playerTransform); // 랜덤 이펙트 생성

        // 오브젝트 초기화
        obj.transform.rotation *= Quaternion.Euler(new Vector3(0, 90f ,0)); 
        obj.transform.position += obj.transform.right * -1f * 1f + Vector3.up * 0.5f;

        obj.transform.SetParent(null);
        Destroy(obj, 0.5f);
    }
}
