using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System;

public enum PlayerState
{
    Idle,
    Idle2,
    Run,
    BasicAttack,
    Unique,
    Hit,
    Die,
}



public class PlayerSpineAnimationController : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public Spine.Animation TargetAnimation { get; private set; }



    
    public PlayerState prevStableState;
    [SerializeField] AnimationReferenceAsset currStableAnimation;

    [SerializeField] EventDataReferenceAsset eventRef_basicAttack;

    public SerializableDictionary<PlayerState, AnimationReferenceAsset > animations;


    public void Init( CharacterDataSO characterData)
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.AnimationState.Event += HandleEvent;


        animations = characterData.animations;
    }


    public void OnMove(float magnitude, float lastMoveDirX)
    {
        bool moved = magnitude >0;
        
        PlayerState newStableState = moved? PlayerState.Run: PlayerState.Idle2;
        if( prevStableState != newStableState)
        {
            PlayNewStableAnimation(newStableState);
            
            prevStableState = newStableState;
        }

        SetFlip(lastMoveDirX);
    }

    public void OnBasicAttackStart()
    {
        PlayTransientAnimation(PlayerState.BasicAttack);

    }

    void PlayEffect()
    {
        Debug.Log("----");
    }

    void HandleEvent (Spine.TrackEntry trackEntry, Spine.Event e) {
        if (e.Data == eventRef_basicAttack.EventData)
        {
            PlayEffect();
        }
            
    }


    public void OnUseUniqueAbility()
    {
        PlayTransientAnimation(PlayerState.Unique);
    }

//===================================================================================

    void PlayNewStableAnimation (PlayerState newState) 
    {
        if( animations.TryGetValue(newState, out var animationReference))
        {
            currStableAnimation = animationReference;
            skeletonAnimation.AnimationState.SetAnimation(0, currStableAnimation, true);
        }
    }

    void PlayTransientAnimation(PlayerState playerState)
    {
        if( animations.TryGetValue(playerState, out var animationReference))
        {
            // Play the shoot animation on track 1.
            TrackEntry attackTrack = skeletonAnimation.AnimationState.SetAnimation(1, animationReference, false);
            // skeletonAnimation.AnimationState.AddAnimation(0,currStableAnimation,true,0);
            attackTrack.MixAttachmentThreshold = 1f;
            attackTrack.SetMixDuration(0f, 0f);
            skeletonAnimation.state.AddEmptyAnimation(1, 0f, 0f);
        }
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



    /// <summary>Play an animation. If a transition animation is defined, the transition is played before the target animation being passed.</summary>
    public void PlayNewAnimation (Spine.Animation target, int layerIndex) 
    {
        Spine.Animation current = null;
        current = GetCurrentAnimation(layerIndex);

        skeletonAnimation.AnimationState.SetAnimation(layerIndex, target, true);
        this.TargetAnimation = target;
    }

    /// <summary>Play a non-looping animation once then continue playing the state animation.</summary>
    public void PlayOneShot (Spine.Animation oneShot, int layerIndex) 
    {
        Spine.AnimationState state = skeletonAnimation.AnimationState;
        state.SetAnimation(0, oneShot, false);
        state.AddAnimation(0, this.TargetAnimation, true, 0f);
    }

    Spine.Animation GetCurrentAnimation (int layerIndex) 
    {
        TrackEntry currentTrackEntry = skeletonAnimation.AnimationState.GetCurrent(layerIndex);
        return (currentTrackEntry != null) ? currentTrackEntry.Animation : null;
    }
	
}
