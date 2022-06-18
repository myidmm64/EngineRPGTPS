using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> monsters = new List<GameObject>();
    [SerializeField]
    private List<Transform> spawnPoints = new List<Transform>();

    private bool isSpwan = false;

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
        for(int i =0; i<spawnPoints.Count; i++)
        {
            int random = Random.Range(0, monsters.Count);
            GameObject monster = Instantiate(monsters[random], spawnPoints[i].position, Quaternion.identity);
            monster.SendMessage("OnCkTarget", target);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1f);
        Destroy(transform.parent.gameObject);
    }
}
