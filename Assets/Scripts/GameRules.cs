using System;
using UnityEngine;

[Serializable]
public class CGameParams
{
    [SerializeField]
    private string _difficulty;
    [SerializeField]
    private int _card_numbers;

    public int CardNumbers => _card_numbers;
    public string Difficulty => _difficulty;
}


[CreateAssetMenu(fileName = "New GameRule", menuName = "Game Rule", order = 10)]
public class GameRules : ScriptableObject
{
    [SerializeField]
    CGameParams[] _rules;

    public CGameParams[] Rules => _rules;
}
