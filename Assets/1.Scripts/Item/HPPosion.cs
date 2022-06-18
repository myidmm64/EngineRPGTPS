using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPPosion : Item
{
    [SerializeField]
    private PlayerDamaged _playerDamage = null; // HP를 올려주기 위한 스크립트 가져오기
    [SerializeField]
    private TextMeshProUGUI _Pricetext = null; // 가격 텍스트
    [SerializeField]
    private int _hpUp = 1; // HP를 얼마나 올려줄 것인지
    

    private void Start()
    {
        // 초기화
        Count = 0;
        Price = 1;
        _Pricetext.SetText($"가격 : {Price}");
    }

    public override void UseItem()
    {
        if(Count > 0) // 갯수가 1개 이상일 때만 적용
        {
            _playerDamage.HP += _hpUp;
            Count--;
        }
    }

    public override void Buy()
    {
        if (Price <= _player.Coin) // 플에이어의 코인이 가격 이상이라면 실행
        {
            Count++;
            _player.Coin -= Price;
            Price+= 1;
            _Pricetext.SetText($"가격 : {Price}");
        }
    }
}
