using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Crosshair : MonoBehaviour
{
    private Vector3 mousePos;

    private void Update()
    {
        GetMousePosition();
    }

    private void GetMousePosition()
    {
        Vector3 pos = Input.mousePosition;

        //mousePos = MainCam.WorldToScreenPoint(pos);

        transform.position = pos;
    }
}
