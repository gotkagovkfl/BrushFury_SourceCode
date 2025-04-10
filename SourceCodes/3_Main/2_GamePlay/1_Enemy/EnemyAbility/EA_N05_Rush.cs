using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using System.Threading;


[CreateAssetMenu(fileName = "EA_N05_Rush", menuName = "SO/EnemyAbility/N05_Rush")]
public class EA_N05_Rush : EnemyAbilitySO
{
    [Header("Extra Setting")]
    // public float radiusWeight = 1.5f;
    public float accelerationWeight = 5f;
    public float impulse = 50;
    public float decelerationDuration = 0.5f;


    public float rushDuration = 1f; // 돌진 지속 시간
    // public float dashSpeed;
     
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
        float height = 10;
        float width = enemy.colliderRadius*2;
        AreaIndicator areaIndicator = PoolManager.Instance.areaIndicatorPoolManager.GetSquare(enemy, initPos, dir, castingTime, width,height );
        
        
        return areaIndicator;
    }


    public override async UniTask ActivationTask(Enemy enemy, Vector3 targetPos, CancellationToken token)
    {
        await RushRoutine(enemy, targetPos, token);
    }


    //===========================================
    private async UniTask RushRoutine( Enemy enemy,Vector3 targetPos, CancellationToken token)
    {
        NavMeshAgent navAgent = enemy.ai.navAgent;
        float originRadius = navAgent.radius;
        float originDmg = enemy.status.pDmg;
        
        Vector3 startPos = enemy.myTransform.position;
        Vector3 dir = (targetPos - startPos).WithFloorHeight().normalized;
        // if(dir==Vector3.zero)
        // {
        //     dir = enemy.ai.lastActionDir;
        // }
        float originSpeedMultiplier = enemy.status.movementSpeedMultiplier;
        float rushMovementSpeed = enemy.status.movementSpeed * accelerationWeight;
        Vector3 endPos = startPos + dir * rushMovementSpeed * rushDuration;
        
        
        

        // 순간 달리기
        navAgent.isStopped = false;

        navAgent.radius = 0.01f;
        enemy.status.pDmg = originDmg * dmgWeight;
        enemy.status.movementSpeedMultiplier += accelerationWeight-1;
        enemy.status.stack_immobilized --;
        enemy.status.stack_ccImmunity++;

        enemy.status.targetPosition = endPos;
        await UniTask.WaitUntil( ()=> navAgent.remainingDistance < 0.1f , cancellationToken: token).SuppressCancellationThrow();
        if( token.IsCancellationRequested ) return;

        //점차적으로 감소
        endPos += dir * rushMovementSpeed;
        enemy.status.targetPosition = endPos;

        float elapsedTime = 0;
        float decelerationPerUpdate = (accelerationWeight-1) / (decelerationDuration / Time.fixedDeltaTime);
        while(elapsedTime < decelerationDuration)
        {
            enemy.status.movementSpeedMultiplier -= decelerationPerUpdate;
            
            elapsedTime += Time.fixedDeltaTime;
            await UniTask.WaitForFixedUpdate(token).SuppressCancellationThrow();
            if(token.IsCancellationRequested) return;
        }

        navAgent.radius = originRadius;
        enemy.status.pDmg= originDmg;
        // enemy.status.movementSpeedMultiplier -= accelerationWeight-1;
        enemy.status.stack_immobilized ++;
        enemy.status.stack_ccImmunity--;

        // enemy.status.stack_canKnockbackPlayer--;
    }



}
