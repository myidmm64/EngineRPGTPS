using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPPosion : Item
{
    [SerializeField]
    private PlayerDamaged _playerDamage = null; // HP�� �÷��ֱ� ���� ��ũ��Ʈ ��������
    [SerializeField]
    private TextMeshProUGUI _Pricetext = null; // ���� �ؽ�Ʈ
    [SerializeField]
    private int _hpUp = 1; // HP�� �󸶳� �÷��� ������
    

    private void Start()
    {
        // �ʱ�ȭ
        Count = 0;
        Price = 1;
        _Pricetext.SetText($"���� : {Price}");
    }

    public override void UseItem()
    {
        if(Count > 0) // ������ 1�� �̻��� ���� ����
        {
            _playerDamage.HP += _hpUp;
            Count--;
        }
    }

    public override void Buy()
    {
        if (Price <= _player.Coin) // �ÿ��̾��� ������ ���� �̻��̶�� ����
        {
            Count++;
            _player.Coin -= Price;
            Price+= 1;
            _Pricetext.SetText($"���� : {Price}");
        }
    }
}
