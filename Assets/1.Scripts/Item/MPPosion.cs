using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPPosion : Item
{
    [SerializeField]
    private PlayerUseSkill _playerUseSkill = null;
    [SerializeField]
    private int _mpUp = 1;

    private void Start()
    {
        Count = 100;
        Price = 1;
    }

    public override void UseItem()
    {
        if (Count > 0)
        {
            _playerUseSkill.MP += _mpUp;
            Count--;
        }
    }

    public override void Buy()
    {
        if(Price <= _player.Coin)
        {
            Count++;
            _player.Coin -= Price;
        }
    }
}
