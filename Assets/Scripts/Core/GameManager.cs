using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _world = null;

    private void Start()
    {
        // ���� �ȿ� �ִ� ��� ������Ʈ�� StartInit�� ������ ����
        _world.BroadcastMessage("StartInit", SendMessageOptions.DontRequireReceiver);
    }
}
