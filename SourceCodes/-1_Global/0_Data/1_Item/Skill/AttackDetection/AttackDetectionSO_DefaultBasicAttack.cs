using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;

[CreateAssetMenu(fileName = "DefaultBasicAttackDetection", menuName = "SO/AttackDetection/DefaultBasicAttack", order = int.MaxValue)]
public class AttackDetectionSO_DefaultBasicAttack : AttackDetectionSO
{
    public float attackRange = 3f; //평타 종류별로 2f 는 달라질 수 잇음. 
    public float radius = 2f;
    
    public override void Detect(Vector3 attackDir, Vector3 effectPos, BasicAttackSO data,float radiusMultiplier=1)
    {

        
        // 공격 방향 계산
        Vector3 spherePosition = (attackDir * attackRange).WithStandardHeight();  

        // 데미지 판정 - 
        Collider[] hits = Physics.OverlapSphere(effectPos + spherePosition, radius*radiusMultiplier, GameConstants.enemyLayer);
        int hitCount = 0;
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.GetDamaged( hit.ClosestPoint( Player.Instance.t.position ), data.finalDmg);
                hitCount++;
            }
        }

        //
        if (hitCount > 0)
        {
            Player.Instance.RecoverInkWithBasicAttack(hitCount);
        }
    }
}
