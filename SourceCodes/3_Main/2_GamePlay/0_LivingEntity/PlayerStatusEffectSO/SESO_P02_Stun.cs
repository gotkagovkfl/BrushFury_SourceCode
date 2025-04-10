using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SE_P02", menuName = "SO/StatusEffect/P02_Stun")]
public class SESO_P02_Stun: PlayerStatusEffectSO
{
    public override string id => "P02";

    public override string dataName => "기절";
}
