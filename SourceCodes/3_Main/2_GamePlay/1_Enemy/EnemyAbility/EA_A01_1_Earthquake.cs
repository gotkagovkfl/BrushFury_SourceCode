using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;
using Cysharp.Threading.Tasks;
using System.Threading;



[CreateAssetMenu(fileName = "EA_A01_1", menuName = "SO/EnemyAbility/A01_1_바람의상처")]
public class EA_A01_1_Earthquake  : EnemyAbilitySO
{
    [Header("Extra")]
    [SerializeField] EnemyProjectile prefab_earthquake;

    public float movementSpeed = 8f;
    public float lifeTime = 5f;
    public float radius = 1;
    


    public override bool UsageConditions(Enemy enemy)
    {
        return true;
    }

    public override Transform GetTarget(Enemy enemy)
    {
        return enemy.t_target;
    }


    public override AreaIndicator ShowCastingIndicator(Enemy enemy,  Vector3 targetPos)
    {
        Vector3 initPos = enemy.myTransform.position;
        Vector3 dir = (targetPos-initPos).normalized;
        float height = movementSpeed* lifeTime;
        AreaIndicator areaIndicator = PoolManager.Instance.areaIndicatorPoolManager.GetSquare(enemy, initPos, dir, castingTime, radius,height );
        
        
        return areaIndicator;
    }

    public override UniTask ActivationTask(Enemy enemy, Vector3 targetPos, CancellationToken token)
    {
        Vector3 initPos = enemy.myTransform.position.WithStandardHeight();
        targetPos = targetPos.WithStandardHeight();
        
        EnemyProjectile ep = PoolManager.Instance.enemyProjectilePoolSys.GetEP(enemy,this, prefab_earthquake, initPos, targetPos);

        
        return UniTask.CompletedTask; 
    }


}
