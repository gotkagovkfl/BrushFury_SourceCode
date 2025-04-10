using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;
using Cysharp.Threading.Tasks;
using System.Threading;

[CreateAssetMenu(fileName = "EA_N02", menuName = "SO/EnemyAbility/N02_자폭")]
public class EA_N02_Explosion  : EnemyAbilitySO
{
    [Header("Explosion Setting")]
    public float explosionRadius = 5f;
    public float impulse = 30;


    [SerializeField] EnemyProjectile prefab_explosion;

    //=======================================================================================
    
    public override Transform GetTarget(Enemy enemy)
    {
       return enemy.t_target;
    }
    public override bool UsageConditions(Enemy enemy)
    {
        return true;
    }

    public override AreaIndicator ShowCastingIndicator(Enemy enemy,  Vector3 targetPos)
    {
        Vector3 initPos = enemy.myTransform.position;
        AreaIndicator areaIndicator = PoolManager.Instance.areaIndicatorPoolManager.GetCircle(enemy, initPos, castingTime, explosionRadius);
        
        return areaIndicator;
    }

    
    public override UniTask ActivationTask(Enemy enemy, Vector3 targetPos, CancellationToken token)
    {    
        // 폭팔 이펙트 생성
        Vector3 initPos = enemy.myTransform.position.WithStandardHeight();
        PoolManager.Instance.enemyProjectilePoolSys.GetEP(enemy, this, prefab_explosion, initPos, initPos);


        // 그리고 자신 파괴
        enemy.CleanDeath();

        
        return UniTask.CompletedTask; 
    }
}
