using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using BW;
using System;
using System.Data.Common;


[System.Serializable]
public class EnemyAbility
{
    public Enemy enemy;
    public EnemyAbilitySO data;

    public int skillIdx;
    
    public int useCount; 
    public float lastUseTime;   // 스킬 마지막 사용시간 

    public float cooltimeRemain => lastUseTime + data.cooltime - Time.time;
    public bool isCooltimeOk => cooltimeRemain <= 0;

    // Coroutine abilityRoutine;
    // Vector3 usingPos;
    public Vector3 targetPos;
    public Vector3 rawDir;

    // public bool isCasting;


    



    public EnemyAbility(EnemyAbilitySO data, Enemy enemy, int skillIdx)
    {
        this.data = data;
        this.enemy = enemy;
        this.skillIdx = skillIdx;
        lastUseTime = -data.cooltime + 1 ; // 획득 후 1초 후에 사용되도록.
    }

    //=================================================================================

    public bool CanSelected()
    {
        return isCooltimeOk == true && data.UsageConditions(enemy)==true;
    }

    public bool CanUse()
    {
        return isCooltimeOk == true 
                && IsTargetInAbilityRange(enemy)
                && data.UsageConditions(enemy)==true;;
    }

    public bool IsTargetInAbilityRange(Enemy enemy)
    {
        return data.IsTargetInAbilityRange(enemy) ;
    }



    public async UniTask CastingTask( CancellationToken token)
    {
        //
        enemy.animationController.PlayAnim_Cast(data);  // 애니메이션
        
        float castingTime = data.castingTime;
        float abilityPosDecisionTime = castingTime * data.targetPositionDecisionRate;
        float focusingTime = castingTime - abilityPosDecisionTime;
        AreaIndicator areaIndicator = data.ShowCastingIndicator(enemy, targetPos);

        //
        Action action = ()=>UpdateAbilityPos(areaIndicator); 
        if( await action.TaskWhile(abilityPosDecisionTime,token).CanNextStep(token)==false) return; 

        await UniTask.WaitForSeconds( focusingTime  , cancellationToken : token).SuppressCancellationThrow();       
    }


    void UpdateAbilityPos(AreaIndicator castingIndicator)
    {
        targetPos = data.GetAbilityTargetPos(enemy);
        rawDir = targetPos - enemy.myTransform.position;

        //
        castingIndicator?.OnUpdateTargetPos(targetPos);
    }

    public async UniTask ActivationTask( CancellationToken token)
    {
        if( await UniTask.WaitForSeconds(data.activationDelay,cancellationToken : token).CanNextStep(token)==false) return;

        useCount ++;
        lastUseTime = Time.time; //시간기록

        // 능력 발동
        SoundManager.Instance.Play(enemy.myTransform, data.sfx_ability);
        await data.ActivationTask(enemy, targetPos, token).SuppressCancellationThrow();                           
    }

    public async UniTask AfterDelayTask( CancellationToken token)
    {
        //
        enemy.animationController.PlayAnim_afterDelay(data);
        await UniTask.WaitForSeconds(data.delay_afterCast, cancellationToken: token).SuppressCancellationThrow();  
    }

    public void OnInterrupted()
    {
        lastUseTime = Time.time - data.cooltime*0.5f;
    }
    

    public void UseOnDeathAbility()
    {
        targetPos = data.GetAbilityTargetPos(enemy);
        data.ActivationTask(enemy, targetPos, new()).Forget();                 // 애니메이션 없이 능력만
    }
}
