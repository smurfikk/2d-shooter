using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTypes", menuName = "Game/Enemy Config")]
public class EnemyTypes : ScriptableObject
{
    [SerializeField] private string _Name;

    public string Name => this._Name;
}
