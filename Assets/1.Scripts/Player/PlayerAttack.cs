using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _attackEffect = new List<GameObject>();
    [SerializeField]
    private Transform _playerTransform = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<SkulMove>()?.Damage();
        }
    }

    //이거 나중에 어딘가로 옮겨

    public void AttackEffectSpawn()
    {
        int random = Random.Range(0, _attackEffect.Count);
        GameObject obj = Instantiate(_attackEffect[random], _playerTransform);
        obj.transform.rotation *= Quaternion.Euler(new Vector3(0, 90f ,0));
        obj.transform.position += obj.transform.right * -1f * 1f + Vector3.up * 0.5f;

        obj.transform.SetParent(null);
        Destroy(obj, 0.5f);
    }
}
