using System;
using UnityEngine;

[Serializable]
public class CardData
{
    [SerializeField]
    private string _identifier;
    [SerializeField]
    private Sprite _sprite;
    [SerializeField]
    private float _rotate_degree;

    public string Identifier => _identifier;
    public Sprite Sprite => _sprite;

    public float RotateDegree => _rotate_degree;
}