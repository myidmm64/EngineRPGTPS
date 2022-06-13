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
        if (isSpwan == false)
        {
            isSpwan = true;
            StartCoroutine(Spawn());
        }
    }

    private IEnumerator Spawn()
    {
        for(int i =0; i<spawnPoints.Count; i++)
        {
            int random = Random.Range(0, monsters.Count);
            Instantiate(monsters[random], spawnPoints[i].position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(gameObject);
    }
}
