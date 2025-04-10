using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;
using Cysharp.Threading.Tasks;
using System.Threading;

[CreateAssetMenu(fileName = "EA_N04_Mortar", menuName = "SO/EnemyAbility/04_Mortar")]
public class EA_N04_Mortar : EnemyAbilitySO
{
    [Header("Mortar Setting")]
    public float count = 3;     //
    
    public float radius = 1f;           // 피격 범위
    public float timeToImpact = 3;     // 탄 도착 시간
    [Range(0.1f ,0.9f)]public float offScreenTimeRatio=0.5f; // 체공 시간중 화면 밖에 있는 비율 
    public float delayPerProjectile=0.1f ;  //포탄 마다 발사 딜레이 
    public float targetPosNoiseRadius = 2f; // 타격지점
    


    [SerializeField] EnemyProjectile prefab_proj;

    //=======================================================================================
    
    public override Transform GetTarget(Enemy enemy)
    {
       return enemy.t_target;
    }
    public override bool UsageConditions(Enemy enemy)
    {
        return true;
    }


    public override AreaIndicator ShowCastingIndicator(Enemy enemy, Vector3 targetPos)
    {
        return null;
    }

    public override async UniTask ActivationTask(Enemy enemy, Vector3 targetPos, CancellationToken token)
    {  
        // 폭발 이펙트 생성0lllllllllll,.
        Vector3 initPos = enemy.myTransform.position.WithStandardHeight();

        for(int i=0;i<count;i++)
        {
            //타겟 포인트 노이즈 
            Vector3 noisedTargetPos = GetNoisedTargetPos(targetPos);    
            
            PoolManager.Instance.enemyProjectilePoolSys.GetEP(enemy, this, prefab_proj, initPos, noisedTargetPos);
            AreaIndicator areaIndicator = PoolManager.Instance.areaIndicatorPoolManager.GetCircle(enemy, noisedTargetPos, timeToImpact, radius, destroyOnEnemyDeath : false);

            await UniTask.WaitForSeconds(delayPerProjectile).SuppressCancellationThrow();
            if (token.IsCancellationRequested) return;  
        }
    }


    Vector3 GetNoisedTargetPos(Vector3 targetPos)
    {
        Vector3 ret = targetPos;

        ret += Math.GetRandomPointOnCircle(targetPosNoiseRadius);

        return ret;
    }


}