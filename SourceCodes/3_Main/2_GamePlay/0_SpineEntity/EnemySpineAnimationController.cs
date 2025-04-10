using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System;
using UnityEngine.Rendering.Universal;
using Cysharp.Threading.Tasks;
using System.Threading;

public class EnemySpineAnimationController : MonoBehaviour
{
    public enum EnemyAnimationState
    {
        Idle,
        Move,
        hit,
        Die,
        Ability,
        onDeathAbility,
    }
    public SpineEntity spineEntity;
    public SkeletonAnimation skeletonAnimation;
    
    public EnemyAnimationState prevStableState;
    [SerializeField] AnimationReferenceAsset currStableAnimation;

    [Space(30)]
    [Header("Animations")]    
    public AnimationReferenceAsset idleAnimaton;
    public AnimationReferenceAsset moveAnimaton;
    public AnimationReferenceAsset deathAnimaton;
    //============================================================================


    public void Init(Enemy enemy)
    {
        spineEntity = GetComponentInChildren<SpineEntity>();
        spineEntity.Init(enemy.ai.navAgent.radius, 0); 
        
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.AnimationState.Event += HandleEvent;


        // 애니메이션 지정
        EnemyDataSO enemyData = enemy.data;
        this.idleAnimaton = enemyData.idleAnimaton;
        this.moveAnimaton = enemyData.moveAnimaton;
        this.deathAnimaton = enemyData.deathAnimaton;
        // this.abilityAnimations = enemyData.abilityAnimations;

        skeletonAnimation.AnimationState.SetEmptyAnimation(1,0);    // 시전 애니메이션 덮기
    }


    public void OnMove(float magnitude, float lastMoveDirX)
    {
        bool moved = magnitude >0;
        
        EnemyAnimationState newStableState = moved? EnemyAnimationState.Move: EnemyAnimationState.Idle;
        if( prevStableState != newStableState)
        {
            PlayNewStableAnimation(newStableState);
            
            prevStableState = newStableState;
        }

        SetFlip(lastMoveDirX);
    }

    public void OnStunned()
    {
        PlayNewStableAnimation(EnemyAnimationState.Idle);
        skeletonAnimation.AnimationState.SetEmptyAnimation(1,0);    // 시전 애니메이션 덮기
    }

    public async UniTask WaitForDeathAnimation()
    {
        TrackEntry trackEntry = PlayTransientAnimation(deathAnimaton);
        await WaitUntilAnimationFinished(  trackEntry );
    }

    public async UniTask WaitForAnimation_AbilityActivation(EnemyAbilitySO abilityData,CancellationToken token)
    {
        TrackEntry trackEntry = PlayTransientAnimation(abilityData.activationAnimation);
        await WaitUntilAnimationFinished(  trackEntry , token).SuppressCancellationThrow();
    }

    public void PlayAnim_Cast(EnemyAbilitySO abilityData)
    {
        TrackEntry trackEntry = PlayTransientAnimation(abilityData.castingAnimation);
        
        // trackEntry.TrackEnd = abilityData.castingTime;
        // trackEntry.Loop = false;
    }

    public void PlayAnim_Ability(EnemyAbilitySO abilityData)
    {
        TrackEntry trackEntry =PlayTransientAnimation(abilityData.activationAnimation);
        // if(trackEntry ==null)
        // {
        //     return;
        // }
        
        // trackEntry.TrackEnd = abilityData.delay_afterCast;
        // trackEntry.Loop = false;
    }

    public void PlayAnim_afterDelay(EnemyAbilitySO abilityData)
    {
        TrackEntry trackEntry = PlayTransientAnimation(abilityData.afterDelayAnimation);
        if(trackEntry ==null)
        {
            return;
        }
        trackEntry.TrackEnd = abilityData.delay_afterCast;
        trackEntry.Loop = false;
    }


    void HandleEvent (Spine.TrackEntry trackEntry, Spine.Event e) 
    {
        // if (e.Data == eventRef_basicAttack.EventData)
        // {
        //     PlayEffect();
        // }        
    }

    public float GetEventTime_Ability(EnemyAbilitySO abilityData, float animationSpeed = 1)
    {
        float ret = abilityData.activationDuration;

        ret /= animationSpeed;


        return ret;
    }


    //===================================================================================

    /// <summary>
    /// 반복하여 재생하는 애니메이션  (움직임 )
    /// </summary>
    /// <param name="newState"></param>
    void PlayNewStableAnimation (EnemyAnimationState newState) 
    {
        AnimationReferenceAsset newAnimation = null;
        if (newState == EnemyAnimationState.Idle)
        {
            newAnimation = idleAnimaton;
        }
        else if (newState == EnemyAnimationState.Move)
        {
            newAnimation = moveAnimaton;
        }
        
        currStableAnimation = newAnimation;
        if (currStableAnimation !=null)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, currStableAnimation, true);
        }
    }

    /// <summary>
    ///  한번만 재생하는 애니메이션
    /// </summary>
    /// <param name="targetAnimation"></param>
    TrackEntry PlayTransientAnimation(AnimationReferenceAsset targetAnimation)
    {
        if (targetAnimation == null)
        {
            return null;
        }
            
        // Play the shoot animation on track 1.
        TrackEntry trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, targetAnimation, false);
        // trackEntry.MixAttachmentThreshold = 1f;
        // trackEntry.SetMixDuration(0f, 0f);
        // skeletonAnimation.state.AddEmptyAnimation(1, 0f, 0f);

        return trackEntry;
    }


    /// <summary>Sets the horizontal flip state of the skeleton based on a nonzero float. If negative, the skeleton is flipped. If positive, the skeleton is not flipped.</summary>
    public void SetFlip (float horizontal) 
    {
        if(GameManager.isPaused)
        {
            return;
        } 
        
        if (horizontal != 0) 
        {
            skeletonAnimation.Skeleton.ScaleX = horizontal > 0 ? 1f : -1f;
        }
    }

    // public IEnumerator WaitUntilAnimationFinished(TrackEntry trackEntry)
    // {
    //     if( trackEntry == null)
    //     {
    //         yield break;
    //     }


    //     // 지역 변수로 isComplete를 두고, TrackEntry.Complete 이벤트에서 true로 설정
    //     bool isComplete = false;
    //     trackEntry.Complete += (entry) => isComplete = true;

    //     yield return new WaitUntil( ()=>isComplete); 
    // }

    public async UniTask  WaitUntilAnimationFinished(TrackEntry trackEntry, CancellationToken token)
    {
        if( trackEntry == null)
        {
            return; 
        }

        await UniTask.WaitForSeconds(trackEntry.AnimationEnd, cancellationToken: token).SuppressCancellationThrow();
        if (token.IsCancellationRequested) return;
    }

    
    public async UniTask  WaitUntilAnimationFinished(TrackEntry trackEntry)
    {
        if( trackEntry == null)
        {
            return; 
        }

        await UniTask.WaitForSeconds(trackEntry.AnimationEnd);
    }


}
