using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityStatusEffect", menuName = "SO/EntityStatusEffect")]
public class EntityStatusEffectSO : GameData
{
    public string _id;
    public string _dataName;
    
    public override string id => _id;

    public override string dataName => _dataName;
}
