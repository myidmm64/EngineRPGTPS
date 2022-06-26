using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MPPosion : Item
{
    [SerializeField]
    private PlayerUseSkill _playerUseSkill = null; // MP를 올려주기 위해 스크립트 가져오기
    [SerializeField]
    private TextMeshProUGUI _Pricetext = null; // 가격 텍스트
    private Button _buyButton = null;
    [SerializeField]
    private Image _cantBuyImage = null;
    [SerializeField]
    private int _mpUp = 1; // MP를 얼마나 올려줄 것인지

    private void Awake()
    {
        _buyButton = _Pricetext.transform.parent.GetComponent<Button>();
    }

    private void Start()
    {
        //초기화
        Count = 0;
        Price = 2;
        _Pricetext.SetText($"가격 : {Price}");
    }

    public override void UseItem()
    {
        if(_playerUseSkill.MP >= _playerUseSkill.MaxMP)
        {
            return;
        }

        if (Count > 0) // 개수가 1 이상일때만 적용
        {
            _playerUseSkill.MP += _mpUp;
            Count--;
        }
    }

    public void MarketInit()
    {
        if (Price <= _player.Coin)
        {
            _buyButton.enabled = true;
            _cantBuyImage.enabled = false;
        }
        else
        {
            _buyButton.enabled = false;
            _cantBuyImage.enabled = true;
        }
    }


    public void BuyEvent()
    {
        if (Price > _player.Coin)
        {
            _buyButton.enabled = false;
            _cantBuyImage.enabled = true;
        }
    }

    public override void Buy()
    {
        if(Price <= _player.Coin) // 만약 플레이어의 코인이 가격보다 많으면
        {
            _buyButton.enabled = true;
            _cantBuyImage.enabled = false;

            Count++;
            _player.Coin -= Price;
            _Pricetext.SetText($"가격 : {Price}");

            BuyEvent();
        }
    }
}
