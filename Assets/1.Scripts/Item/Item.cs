using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI _text = null; // 아이템의 개수를 표시해주는 텍스트
    [SerializeField]
    protected Player _player = null; // 플레이어에 관해 할 게 있으면 사용

    private int _price = 1; // 아이템의 가격
    public int Price { get => _price; set => _price = value; } // 아이템의 가격 getset

    private int _count = 0; // 아이템의 개수
    public int Count { 
        get => _count; 
        set
        {
            _count = value;
            _text?.SetText($"{_count}");
        }
    }

    public abstract void UseItem(); // 아이템 사용
    public abstract void Buy(); // 아이템 구매
}
