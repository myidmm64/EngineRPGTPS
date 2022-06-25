using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using static Define;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _attackEffect = new List<GameObject>(); // 어택 이펙트 리스트

    [SerializeField]
    private GameObject _criticalEffect = null;

    [SerializeField]
    [Range(0f, 100f)]
    private float _critical = 20f;

    [SerializeField]
    private TextMeshProUGUI _criticalText = null;
    [SerializeField]
    private RectTransform textTrans = null;

    [SerializeField]
    private GameObject _finalAttackEffect = null;

    [SerializeField]
    private Transform _playerTransform = null; // 플레이어의 트랜스폼
    private Player _player = null; // 
    private PlayerInputs _playerInput = null; // 기본공격 함수 가져오기

    [SerializeField]
    private int _damage = 10; // 검의 데미지
    public int Damage { get => _damage; set => _damage = value; }

    private Sequence _criticalSeq = null;

    private void Awake() 
    {
        //플레이어 트랜스폼 붙어있으면 가져오기
        if (_playerTransform == null) return;

        _player = _playerTransform.GetComponent<Player>();
        _playerInput = _player.GetComponent<PlayerInputs>();

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy")) 
        {
            other.GetComponent<SkulMove>()?.Damage(damage : _damage); //닿은 녀석에게 데미지 주기
        }
    }

    private void Update()
    {
        if (_player == null) return; // 만약 트랜스폼을 넣어주지 않았으면 안함

        if(Input.GetMouseButtonDown(0))
        {
            if(_player.AttackCnt > 0)
            {
                AttackReady(_player.AttackCnt);
            }

        }
    }

    private void AttackReady(int idx)
    {
        switch (idx)
        {
            case 1: // 2번째 공격부터
                _playerInput.MeleeAttack();
                break;
            case 2:
                _playerInput.MeleeAttack();
                break;
            case 3:
                _playerInput.MeleeAttack();
                break;
            default:
                break;
        }

    }

    GameObject obj = null;

    public void AttackEffectSpawn(int value) // 어택 인덱스마다 다른 오브젝트나 그런 거 생성
    {
        bool isCritical = false;

        int random = Random.Range(0, _attackEffect.Count); // 이펙트 인덱스 랜덤으로

        if (Random.Range(0, 100f) < _critical)
        {
            isCritical = true;
            obj = Instantiate(_criticalEffect, _playerTransform); // 크리티컬 이펙트

            _criticalText.enabled = false;
            _criticalSeq.Kill();

            Vector3 currentPos = _criticalText.rectTransform.position;
            _criticalText.rectTransform.anchoredPosition = new Vector3(220f, -50f, currentPos.z);

            _criticalSeq = DOTween.Sequence();

            _criticalText.enabled = true;
            _criticalSeq.Append(_criticalText.rectTransform.DOAnchorPosY(_criticalText.rectTransform.rect.y, 0.3f).SetEase(Ease.OutBack));
            _criticalSeq.AppendCallback(() => 
            {
                _criticalText.enabled = false;
            });
        }
        else
            obj = Instantiate(_attackEffect[Random.Range(0, _attackEffect.Count)], _playerTransform); // 랜덤 이펙트 생성


        if (value == 4)
        {
            obj.AddComponent<EffectMove>();
        }


        // 오브젝트 초기화
        float rand = Random.Range(0.3f, 0.45f);
        obj.transform.localScale = Vector3.one * rand;
        obj.transform.rotation *= Quaternion.Euler(new Vector3(0, 90f, 0));
        switch (value)
        {
            case 1:
                obj.GetComponent<PlayerAttack>().Damage = _damage;
                Destroy(obj, 0.5f);
                break;
            case 2:
                obj.transform.rotation *= Quaternion.Euler(new Vector3(45f, 0, 0));
                obj.transform.localScale = Vector3.one * Random.Range(0.45f, 0.55f);
                obj.GetComponent<PlayerAttack>().Damage = (int)(_damage * 1.2f);
                Destroy(obj, 0.5f);
                break;
            case 3:
                obj.transform.rotation *= Quaternion.Euler(new Vector3(-35f, 0, 0));
                obj.transform.localScale = Vector3.one * Random.Range(0.45f, 0.55f);
                obj.GetComponent<PlayerAttack>().Damage = (int)(_damage * 1.5f);
                Destroy(obj, 0.5f);
                break;
            case 4:
                Destroy(obj, 1f);
                break;
            default:
                break;
        }

        if(isCritical)
            obj.GetComponent<PlayerAttack>().Damage = (int)(_damage * 2f);

        obj.transform.position += obj.transform.right * -1f * 1f + Vector3.up * 0.5f;

        obj.transform.SetParent(null);

    }
}
