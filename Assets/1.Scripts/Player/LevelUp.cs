using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelUp : MonoBehaviour
{
    [SerializeField]
    private int _currentExp = 0; // ���� ����ġ
    [SerializeField]
    private int _targetExp = 5; // ������ ���� �ʿ��� ����ġ
    [SerializeField]
    private int _currentlevel = 1; // ���� ����
    private int _residueExp = 0; //���������� ���� ����ġ

    [field: SerializeField]
    private UnityEvent OnLevelUp = null; // �������� ���� �� ����� �̺�Ʈ
    [field: SerializeField]
    private UnityEvent OnExpUp = null; // ����ġ�� �ö��� �� ����� �̺�Ʈ


    private void Start()
    {
        // �ʱ�ȭ
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
        GUI.Label(new Rect(10, 140, 100, 200), $"���� ����ġ : {_currentExp}", gUI);
        GUI.Label(new Rect(10, 180, 100, 200), $"���� ����ġ : {_residueExp}", gUI);
        GUI.Label(new Rect(10, 220, 100, 200), $"���� ���� : {_currentlevel}", gUI);
    }

    public void ExpUp(int value)
    {
        _currentExp += value;
        _residueExp = _targetExp - _currentExp;

        if (_currentExp >= _targetExp) // ������
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
