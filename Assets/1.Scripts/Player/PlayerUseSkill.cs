using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseSkill : MonoBehaviour
{
    [SerializeField]
    private int _mp = 10;
    public int MP { get => _mp; set => _mp = value; }


    private void OnGUI()
    {
        GUIStyle gUI = new GUIStyle();
        gUI.fontSize = 50;
        gUI.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect(10, 40, 100, 200), $"MP : {_mp}", gUI);
    }
}
