using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

public abstract class EnemyAbilitySO : ScriptableObject
{
    public string id;
    public string skillName;
    public Sprite icon;
    //
    [Header("InGameSetting")]
    public float range;
    public float minRange;  // 최소사거리
    public int priority;    // 우선순위 : 높을 수록 먼저 시전.
    public float cooltime;      //쿨타임 ( 초 ) 
    public bool uninterruptible;    // 캐스팅을 끊을 수 있는 지. 
    public float dmgWeight = 1;
    public bool castWhileMoving;
    public float tenacityOnCasting;


    //
    [Range(0,1)] public float targetPositionDecisionRate;   // 대상 위치가 결정되는 시간 ( 시전시간에서의 비율 )
    public bool showCastingIndicator;   // 시전 시 범위 표시 
    

    // public AnimationClip castingAnimation;
    // public AnimationClip activationAnimation;
    public AnimationReferenceAsset castingAnimation;
    public AnimationReferenceAsset activationAnimation;
    public AnimationReferenceAsset afterDelayAnimation;

    //
    [Space(10)]
    public float castingTime;  // 기술 시전시간 (castingAnimation과 동시에 진행됨.)
    [ReadOnly] public float activationDuration; // activationAnimation 길이
    [ReadOnly] public float activationDelay; // activationAnimation 진행하면서 딜레이 지정이 필요할때, 
    public float delay_afterCast;   // 후딜 ( activationAnimation 진행 후에, )
     

    [Header("Sound")]
    public SoundSO sfx_ability;


    // ============================================================================================
    void OnValidate()
    {
        SetApplyDelay();

    }

    void SetApplyDelay()
    {
        if ( activationAnimation == null || activationAnimation.Animation == null)
        {
            return;
        }
            
        Spine.Animation animation = activationAnimation.Animation;
        activationDelay = animation.Duration;
        Debug.Log($"이벤트 '{animation.Name}'길이 {activationDelay}");
        
        // 타임라인 탐색
        foreach (var timeline in animation.Timelines)
        {
            if (timeline is EventTimeline eventTimeline)
            {
                float[] frames = eventTimeline.Frames; // 이벤트 발생 시간(초 단위)
                Spine.Event[] events = eventTimeline.Events; // 이벤트 정보 배열

                for (int i = 0; i < frames.Length; i++)
                {
                    float time = frames[i];
                    Spine.Event spineEvent = events[i];

                    // 이벤트 이름과 발생 시간을 로그로 출력
                    Debug.Log($"이벤트 '{spineEvent.Data.Name}'는 {time}초에 발생");
                    activationDuration = time;
                    break;
                }
            }
        }
    }




    //================================================================================================================

    public abstract bool UsageConditions(Enemy enemy);
    public abstract AreaIndicator ShowCastingIndicator(Enemy enemy,  Vector3 targetPos);
    public abstract UniTask ActivationTask(Enemy enemy, Vector3 targetPos, CancellationToken token);

    
    //
    public float AbilityDmg(Enemy enemy)
    {
        return enemy.status.pDmg * dmgWeight;
    }

    public abstract Transform GetTarget(Enemy enemy);  

    public bool IsTargetInAbilityRange(Enemy enemy)
    {
        float currRange = range;
        if( currRange>=5f) // 5f 이상은 원거리 취급
        {
            currRange *= enemy.status.rangeWeight;
        }  
        
        return enemy.targetDistSqr <= currRange*currRange;
    }

    public bool IsTargetInMinRange(Enemy enemy)
    {
        float currRange = minRange;
        return enemy.targetDistSqr <= currRange*currRange;
    }



    public Vector3 GetAbilityTargetPos(Enemy enemy)
    {
        Transform target = GetTarget(enemy);
        Vector3 targetPos = target.position;

        if ( IsTargetInAbilityRange(enemy) )   
        {
            // 최소 사거리 안일 떈, 사거리 보정     
            if (minRange>0 && IsTargetInMinRange(enemy))
            {
                Vector3 dir = (targetPos - enemy.myTransform.position).normalized;    // 최소 사거리 안인 경우, 최소 사거리에서 발동시킴. 
                return enemy.myTransform.position+ dir * minRange; 
            }
        
            return targetPos; 
        }
        else
        {
            Vector3 dir = (targetPos - enemy.myTransform.position).normalized;    // 거리밖인 경우, 최대 사거리에서 발동시킴. 
            return enemy.myTransform.position+ dir * range;
        }
    }


    
    

}
