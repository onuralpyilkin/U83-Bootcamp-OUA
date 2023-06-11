using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combo", menuName = "Combo")]
public class Combo : ScriptableObject
{
    public string comboName;
    public List<Attack> attacks = new List<Attack>();
    private int comboLength = 0;

    public void Initialize()
    {
        comboLength = attacks.Count;
        for (int i = 0; i < attacks.Count; i++)
        {
            attacks[i].Initialize();
        }
    }

    public int GetComboLength()
    {
        return comboLength;
    }
}
