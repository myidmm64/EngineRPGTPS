using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPosion : Item
{
    [SerializeField]
    private PlayerDamaged _playerDamage = null;
    [SerializeField]
    private int _hpUp = 1;
    

    private void Start()
    {
        Count = 100;
        Price = 1;
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
        }
    }
}
