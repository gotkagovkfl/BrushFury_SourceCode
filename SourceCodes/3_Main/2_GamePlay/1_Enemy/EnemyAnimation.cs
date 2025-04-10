using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    public SpriteEntity spriteEntity;
    Enemy enemy;
    
    Sequence seq_alive;

    // bool canPlayAliveAnim => (enemy.status.isStunned || enemy.abilitySystem.isUsing) ==false;



    static int hash_defaultState = Animator.StringToHash("SetDefault");
    static List<int> hash_cast = new(){
                                        Animator.StringToHash("Cast_0"),
                                        Animator.StringToHash("Cast_1"),
                                        Animator.StringToHash("Cast_2"),
                                        Animator.StringToHash("Cast_3")
                                    };
    static List<int> hash_activate = new(){
                                        Animator.StringToHash("Activate_0"),
                                        Animator.StringToHash("Activate_1"),
                                        Animator.StringToHash("Activate_2"),
                                        Animator.StringToHash("Activate_3")
                                    };
    //

    public bool canUpdateSpriteDir;

    //========================================================================
    public void Init(Enemy enemy)
    {
        this.enemy = enemy;

        spriteEntity = GetComponent<SpriteEntity>();
        spriteEntity.Init(enemy.data.sprite, enemy.ai.navAgent.radius, enemy.ai.navAgent.height);


        animator = GetComponentInChildren<Animator>();

        //
        InitAnimator();


        //
        PlayAnim_Alive();



    }


    public void OnUpdate(  Vector3 actionDir)
    {
        // 움직임 애니메이션
        // if (seq_alive.IsPlaying() )
        // {
        //     if( canPlayAliveAnim==false)
        //     {
        //         seq_alive.Pause();
        //     }
        // }
        // else
        // {
        //     if( canPlayAliveAnim )
        //     {
        //         seq_alive.Play();
        //     }
        // }

        // 스프라이트 방향
        UpdateSpriteDir(actionDir.x);
    }

    public void OnDie()
    {
        seq_alive.Kill();
    }

    //===========================================================================================


    /// <summary>
    /// targetPos 방향으로 스프라이트 방향을 세팅한다. 
    /// </summary>
    /// <param name="targetPos"></param>
    public void UpdateSpriteDir(float xDirValue)
    {
        spriteEntity.Flip(xDirValue);
    }


    /// <summary>
    /// 사망 애니메이션
    /// </summary>
    /// <returns></returns>
    public IEnumerator DefaultDeathSequence()
    {
        Sequence seq_death = DOTween.Sequence()
            .AppendInterval(0.3f)

            .Append(spriteEntity.spriteRenderer.DOFade(0, 1f))
            .Play();
        
        yield return seq_death.WaitForCompletion();
    }


    void PlayAnim_Alive()
    {
        if( seq_alive!= null &&seq_alive.IsActive())
        {
            seq_alive.Kill();
        }
        
        //
        Vector3 originScale = enemy.myTransform.localScale;
        float targetMultiplier = 1.2f;
        //
        seq_alive = DOTween.Sequence()
        .OnKill( ()=>enemy.myTransform.localScale = originScale )
        .Append(enemy.myTransform.DOScaleY(originScale.x * targetMultiplier,0.5f))
        // .AppendInterval(0.5f)
        .SetLoops(-1, LoopType.Yoyo)
        .Play();
    }

    void InitAnimator()
    {
        // AnimatorOverrideController baseOverrideController = enemy.data.abilityAnimator;
        // RuntimeAnimatorController enemyAnimator = enemy.data.animator;
        // if (  enemyAnimator == null )
        // {
        //     return;
        // }

        // AnimatorOverrideController overrideController =new AnimatorOverrideController(enemy.data.animator);
        // animator.runtimeAnimatorController = overrideController;

        // AnimationClipOverrides clipOverrides = new AnimationClipOverrides(overrideController.overridesCount);
        // overrideController.GetOverrides(clipOverrides);


        // for( int i=0;i<enemy.ai.abilitySystem.abilities.Count;i++ )
        // {   
        //     EnemyAbility ability = enemy.ai.abilitySystem.abilities[i];
            // AnimationClip castingAnimation = ability.data.castingAnimation;
            // AnimationClip activationAnimation = ability.data.activationAnimation;

            // clipOverrides[$"EnemyAnim_Cast_{i}"] = castingAnimation;
            // clipOverrides[$"EnemyAnim_Activate_{i}"] = activationAnimation;
        // }

        // clipOverrides["EnemyAnim_Default"] = enemy.data.defaultAnimation;
        // overrideController.ApplyOverrides(clipOverrides);
    }

    //========================================================
    public void PlayCastingAnim(int abilityIdx)
    {
        int hash = hash_cast[abilityIdx];
        
        animator.SetTrigger(hash);
    }

    public void PlayActivationAnim(int abilityIdx)
    {
        int hash = hash_activate[abilityIdx];

        animator.SetTrigger(hash);
    }

    public void SetDefaultState()
    {
        int hash = hash_defaultState;
        animator.SetTrigger(hash);
    }
}







public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int capacity) : base(capacity) {}

    public AnimationClip this[string name]
    {
        get { return this.Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}
