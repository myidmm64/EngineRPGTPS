using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MPPosion : Item
{
    [SerializeField]
    private PlayerUseSkill _playerUseSkill = null; // MP�� �÷��ֱ� ���� ��ũ��Ʈ ��������
    [SerializeField]
    private TextMeshProUGUI _Pricetext = null; // ���� �ؽ�Ʈ
    [SerializeField]
    private int _mpUp = 1; // MP�� �󸶳� �÷��� ������

    private void Start()
    {
        //�ʱ�ȭ
        Count = 0;
        Price = 1;
        _Pricetext.SetText($"���� : {Price}");
    }

    public override void UseItem()
    {
        if (Count > 0) // ������ 1 �̻��϶��� ����
        {
            _playerUseSkill.MP += _mpUp;
            Count--;
        }
    }

    public override void Buy()
    {
        if(Price <= _player.Coin) // ���� �÷��̾��� ������ ���ݺ��� ������
        {
            Count++;
            _player.Coin -= Price;
            _Pricetext.SetText($"���� : {Price}");
        }
    }
}
