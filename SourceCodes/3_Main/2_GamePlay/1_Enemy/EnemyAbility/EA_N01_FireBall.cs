using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;
using Cysharp.Threading.Tasks;
using System.Threading;

[CreateAssetMenu(fileName = "EA_N01", menuName = "SO/EnemyAbility/N01_불덩이")]
public class EA_N01_FireBall  : EnemyAbilitySO
{
    [Header("Extra")]
    [SerializeField] EnemyProjectile prefab_fireBall;
    public float movementSpeed;
    public float lifeTime;
    

    //==================================================================================
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

    public override UniTask ActivationTask(Enemy enemy, Vector3 targetPos, CancellationToken token)
    {
        Vector3 initPos = enemy.myTransform.position.WithStandardHeight();
        targetPos = targetPos.WithStandardHeight();
        
        EnemyProjectile ep = PoolManager.Instance.enemyProjectilePoolSys.GetEP(enemy,this, prefab_fireBall, initPos, targetPos);

        
        return UniTask.CompletedTask; 
    }


    
}