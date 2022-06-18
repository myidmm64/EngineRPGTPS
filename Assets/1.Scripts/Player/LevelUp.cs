using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelUp : MonoBehaviour
{
    [SerializeField]
    private int _currentExp = 0; // 현재 경험치
    [SerializeField]
    private int _targetExp = 5; // 레벨업 위해 필요한 경험치
    [SerializeField]
    private int _currentlevel = 1; // 현재 레벨
    private int _residueExp = 0; //레벨업까지 남은 경험치

    [field: SerializeField]
    private UnityEvent OnLevelUp = null; // 레벨업을 했을 때 발행될 이벤트
    [field: SerializeField]
    private UnityEvent OnExpUp = null; // 경험치가 올라갔을 때 발행될 이벤트


    private void Start()
    {
        // 초기화
        _currentExp = 0;
        _currentlevel = 1;
        _residueExp = _targetExp - _currentExp;
    }

    private void OnGUI()
    {
        GUIStyle gUI = new GUIStyle();
        gUI.fontSize = 50;
        gUI.fontStyle = FontStyle.Bold;
        gUI.normal.textColor = Color.red;
        GUI.Label(new Rect(10, 140, 100, 200), $"현재 경험치 : {_currentExp}", gUI);
        GUI.Label(new Rect(10, 180, 100, 200), $"남은 경험치 : {_residueExp}", gUI);
        GUI.Label(new Rect(10, 220, 100, 200), $"현재 레벨 : {_currentlevel}", gUI);
    }

    public void ExpUp(int value)
    {
        _currentExp += value;
        _residueExp = _targetExp - _currentExp;

        if (_currentExp >= _targetExp) // 레벨업
        {
            _currentExp = 0;
            _currentExp += _residueExp;
            _targetExp += 2;
            _residueExp = _targetExp;
            _currentlevel++;
            OnLevelUp?.Invoke();
        }

        OnExpUp?.Invoke(); 
    }

}
