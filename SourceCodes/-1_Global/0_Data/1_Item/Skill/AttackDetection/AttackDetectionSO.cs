using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackDetectionSO : ScriptableObject
{
    public float defaultDamage;

    
    public float dmg =>  Player.Instance.status.GetFinalPDmg(defaultDamage, 1f );
    public abstract void Detect(Vector3 attackDir, Vector3 effectPos, BasicAttackSO data, float radiusMultiplier=1);
}
