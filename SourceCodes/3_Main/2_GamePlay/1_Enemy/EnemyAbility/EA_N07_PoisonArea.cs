using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;
using Cysharp.Threading.Tasks;
using System.Threading;

[CreateAssetMenu(fileName = "EA_N07_PoisonArea", menuName = "SO/EnemyAbility/N07_PoisonArea")]
public class EA_N07_PoisonArea :  EnemyAbilitySO
{
    [Header("Extra")]
    public float radius = 3;
    public float slowAmount = 20;
    public float duration = 7f;

    [SerializeField] EnemyProjectile prefab_poisonArea; 
    
    // public override bool ActivationConditions(Enemy enemy)
    // {
    //     return true;
    // }

    public override bool UsageConditions(Enemy enemy)
    {
        return true;
    }



    public override AreaIndicator ShowCastingIndicator(Enemy enemy, Vector3 targetPos)
    {
        return null;
    }


    public override Transform GetTarget(Enemy enemy)
    {
        return enemy.t_target;
    }

    //
    public override UniTask ActivationTask(Enemy enemy, Vector3 targetPos, CancellationToken token)
    {
        Vector3 initPos = targetPos.WithFloorHeight();
        PoolManager.Instance.enemyProjectilePoolSys.GetEP(enemy, this, prefab_poisonArea, initPos, initPos);

        return UniTask.CompletedTask; 
    }


    
}
