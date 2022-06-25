using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _world = null;

    private void Start()
    {
        // 월드 안에 있는 모든 오브젝트에 StartInit이 있으면 실행
        _world.BroadcastMessage("StartInit", SendMessageOptions.DontRequireReceiver);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(2))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
