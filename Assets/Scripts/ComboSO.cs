using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combo", menuName = "Combo")]
public class Combo : ScriptableObject
{
    public string ComboName;
    public List<Attack> Attacks = new List<Attack>();
    private int comboLength = 0;

    public void Initialize()
    {
        comboLength = Attacks.Count;
        for (int i = 0; i < Attacks.Count; i++)
        {
            Attacks[i].Initialize();
        }
    }

    public int GetComboLength()
    {
        return comboLength;
    }
}
