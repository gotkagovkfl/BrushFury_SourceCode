using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;
using Cysharp.Threading.Tasks;
using System.Threading;



[CreateAssetMenu(fileName = "EA_N06", menuName = "SO/EnemyAbility/N06_바닥꿍")]
public class EA_N06_GroundSmash :  EnemyAbilitySO
{
    [Header("Extra")]
    public float radius = 3;
    public float impulse = 20;
    
    [SerializeField] EnemyProjectile prefab_groundSmash; 
    
    // public override bool ActivationConditions(Enemy enemy)
    // {
    //     return true;
    // }

    public override bool UsageConditions(Enemy enemy)
    {
        return true;
    }

    public override AreaIndicator ShowCastingIndicator(Enemy enemy,  Vector3 targetPos)
    {
        Vector3 initPos = targetPos.WithStandardHeight();
        AreaIndicator areaIndicator = PoolManager.Instance.areaIndicatorPoolManager.GetCircle(enemy, initPos, castingTime, radius);
        
        return areaIndicator;
    }


        public override Transform GetTarget(Enemy enemy)
    {
        return enemy.t_target;
    }

    public override UniTask ActivationTask(Enemy enemy, Vector3 targetPos, CancellationToken token)
    {
        Vector3 initPos = targetPos.WithFloorHeight();
        PoolManager.Instance.enemyProjectilePoolSys.GetEP(enemy, this, prefab_groundSmash, initPos, initPos);
        
        return UniTask.CompletedTask; 
    }




}


