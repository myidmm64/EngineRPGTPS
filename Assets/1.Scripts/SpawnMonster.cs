using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> monsters = new List<GameObject>(); // 스폰할 몬스터 종류
    [SerializeField]
    private List<Transform> spawnPoints = new List<Transform>(); // 스폰 위치

    private bool isSpwan = false; // 스폰중인가

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isSpwan == false)
            {
                isSpwan = true;
                StartCoroutine(Spawn(other.gameObject));
            }
        }
    }

    private IEnumerator Spawn(GameObject target)
    {
        for(int i =0; i<spawnPoints.Count; i++) // 몬스터 소환
        {
            int random = Random.Range(0, monsters.Count);
            GameObject monster = Instantiate(monsters[random], spawnPoints[i].position, Quaternion.identity);
            monster.SendMessage("OnCkTarget", target); // 플레이어로 타겟 설정
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1f);
        Destroy(transform.parent.gameObject);
    }
}
