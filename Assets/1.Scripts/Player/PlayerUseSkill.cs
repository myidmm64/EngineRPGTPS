using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUseSkill : MonoBehaviour
{
    [SerializeField]
    private int _mp = 10; // ¸¶³ª
    public int MP 
    {
        get => _mp;

        set
        {
            _mp = value;

            _mpSlider.value = (float)_mp / _maxMP;
        }
    }
    [SerializeField]
    private int _maxMP = 100;
    public int MaxMP
    {
        get => _maxMP;

        set
        {
            _maxMP = value;

            _mpSlider.value = (float)_mp / _maxMP;
        }
    }

    [SerializeField]
    private float _cooltime = 10f;
    [SerializeField]
    private float _currentTime = 10f;
    [SerializeField]
    private bool _isSkillable = true;
    [SerializeField]
    private Transform[] _shotPositions = null;
    [SerializeField]
    private GameObject _bulletPrefab = null;
    [SerializeField]
    private TextMeshProUGUI _cooltimeText = null;
    [SerializeField]
    private Image _fade = null;

    private Player _player = null;

    [SerializeField]
    private Slider _mpSlider = null;

    /*private void OnGUI()
    {
        GUIStyle gUI = new GUIStyle();
        gUI.fontSize = 50;
        gUI.fontStyle = FontStyle.Bold;
        gUI.normal.textColor = Color.red;
        GUI.Label(new Rect(10, 20, 100, 200), $"MP : {_mp}", gUI);
    }*/

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _currentTime = _cooltime;
        MP = MaxMP;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        _currentTime = Mathf.Clamp(_currentTime, 0f, 10f);

        if(_isSkillable)
        {
            _fade.enabled = false;
            _cooltimeText.SetText($"");
        }
        else
        {
            _fade.enabled = true;
            _cooltimeText.SetText($"{(_cooltime - _currentTime).ToString("N1")}");
        }

        if(_currentTime >= _cooltime)
        {
            _isSkillable = true;
        }

        if(_isSkillable)
        {
            if (MP < 15)
                return;

            if (Input.GetKeyDown(KeyCode.R))
            {
                MP -= 15;
                StartCoroutine(UseSkill());
            }
        }

    }

    private IEnumerator UseSkill()
    {

        _player.HeadRotate(true);
        Player.PlayerState curState = _player.playerState;
        _player.SetState(Player.PlayerState.Skilling);
        for (int i = 0; i < _shotPositions.Length; i++)
        {
            GameObject obj = Instantiate(_bulletPrefab, _shotPositions[i].position, transform.rotation);
            obj.transform.SetParent(null);
            obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.4f);

            yield return new WaitForSeconds(0.1f);
        }
        _player.SetState(curState);

        _isSkillable = false;
        _currentTime = 0f;
    }
}
