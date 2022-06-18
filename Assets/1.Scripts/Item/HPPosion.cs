using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPPosion : Item
{
    [SerializeField]
    private PlayerDamaged _playerDamage = null;
    [SerializeField]
    private TextMeshProUGUI _Pricetext = null;
    [SerializeField]
    private int _hpUp = 1;
    

    private void Start()
    {
        Count = 0;
        Price = 1;
        _Pricetext.SetText($"가격 : {Price}");
    }

    public override void UseItem()
    {
        if(Count > 0)
        {
            _playerDamage.HP += _hpUp;
            Count--;
        }
    }

    public override void Buy()
    {
        if (Price <= _player.Coin)
        {
            Count++;
            _player.Coin -= Price;
            Price+= 1;
            _Pricetext.SetText($"가격 : {Price}");
        }
    }
}
