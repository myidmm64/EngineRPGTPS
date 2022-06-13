using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveAble
{
    public bool IsStop { get; set; } // 멈춰야되는지

    public void Move(); // 전반적인 이동 함수
}
