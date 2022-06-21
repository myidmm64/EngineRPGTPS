using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulAttack : MonoBehaviour
{
    [SerializeField]
    private int _damage = 1;
    public int Damage
    {
        get => _damage;
        set
        {
            _damage = value;
        }
    }
}
