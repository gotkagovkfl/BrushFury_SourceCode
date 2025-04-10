using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using DG.Tweening;
using BW;
using Unity.VisualScripting;

using Cysharp.Threading.Tasks;
using System.Threading;
using Cysharp.Threading.Tasks.Triggers;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAI))]
public abstract class Enemy : BwPoolObject
{
    public EnemyDataSO data; //적의 데이터 
    [SerializeField] EnemySpawner spawner;
    
    public override PoolType poolType => PoolType.Enemy; 
    protected override string id => data.id;
    
    public EnemyAbilitySystem abilitySystem;
    public EnemyStatus status;
    public EnemyStatusEffects statusEffects;
    // public EnemyAnimation eAnimation;
    public EnemySpineAnimationController animationController;
    // EnemyStateUI stateUI;

    public EnemyAI ai;

    CapsuleCollider enemyCollider;
    public float colliderRadius=>enemyCollider.radius;
    public Rigidbody rb;

    public Transform t_target;
    public float targetDistSqr;


    public bool isAlive => activated == true && status.currHp > 0;
    bool activated;

    //
    public Vector3 lastHitPoint;

    public bool canBodyAttack;



    // ai update
    [SerializeField] float reationRate = 0.2f;
    [SerializeField] float lastAiUpdateTime = -2f;
    bool canUpdateAI => Time.time >= lastAiUpdateTime + reationRate; 
    public Vector3 lastActionDir;


    //========================================= 
    //  ======================

    void Update()
    {
        // 
        if ( isAlive == false || GamePlayManager.isGamePlaying == false)
        {
            return;
        }

        Vector3 dir = t_target.position - myTransform.position;
        targetDistSqr = dir.sqrMagnitude;

        //
        statusEffects.OnUpdate();

        if( TryUpdateAi())
        {

        }

        //
        lastActionDir =  GetActionDir();
        animationController.OnMove(ai.navAgent.velocity.magnitude, lastActionDir.x);
    }


    /// <summary>
    /// 공격 & 이동 지점 결정  
    /// </summary>
    /// <returns></returns>
    bool TryUpdateAi()
    {
        if (activated && canUpdateAI  && abilitySystem.isUsing==false)
        {
            lastAiUpdateTime = Time.time;
            if(abilitySystem.TryStartAbilityRoutine() == false)
            {
                ai.OnUpdate(abilitySystem);
                return true;
            }
        }
        return false;
    }


    


    //===========================

    /// <summary>
    ///  이게 init 보다 먼저 호출됨.
    /// </summary>
    public override void OnCreatedInPool()
    {
        enemyCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        ai = GetComponent<EnemyAI>();
        statusEffects = GetComponent<EnemyStatusEffects>();
        // eAnimation = GetComponent<EnemyAnimation>();
        animationController = GetComponentInChildren<EnemySpineAnimationController>();
        abilitySystem = GetComponentInChildren<EnemyAbilitySystem>();
        

        spawner = GetComponentInChildren<EnemySpawner>();

        GamePlayManager.Instance.event_stageClear += CleanDeath ;
    }

    public override void OnGettingFromPool()
    {

        rb.velocity = Vector3.zero;
    }


    //=====================================
    /// <summary>
    /// 척 스텟 초기화 - pool에서 생성되거나, 재탕될 때 호출됨. 
    /// </summary>
    /// <param name="data"></param>
    public void Init()
    {
        status = new(data);
        statusEffects.Init(this);
        
        // data 에 따라 radius 및 이동속도 도 세팅해야함. 
        abilitySystem.Init(this);
        ai.Init(this);
        

        //
        Init_Custom();

        //
        t_target = Player.Instance.transform;
        Vector3 dir = t_target.position - myTransform.position;
        targetDistSqr = dir.sqrMagnitude;


        //
        animationController.Init(this);
        spawner.Init(ai.navAgent.radius);
        //
        
        StartCoroutine(ActivationRoutine());
    }




    IEnumerator ActivationRoutine()
    {
        activated = false;
        enemyCollider.enabled = false;
        animationController.spineEntity.mr.enabled = false;
        
        //
        yield return spawner.SpawnEffect();
        
        //
        if( Stage.Instance.isFinished)
        {
            yield break;
        }

        activated = true;
        enemyCollider.enabled = true;
        animationController.spineEntity.mr.enabled = true;

        ai.OnActiavted();
    }


    protected abstract void Init_Custom();
    protected abstract void OnHit();
    protected abstract void OnDie_Custom();


    //====================================================================================== 

    
    Vector3 GetActionDir()
    {
        if ( abilitySystem.isUsing )
        {
            return abilitySystem.usingAbility.rawDir;
        }
        else
        {
            return status.targetPosition - myTransform.position;
            
        }
    }


    public void GetDamaged( Vector3 hitPoint, float damage, float impulse  = 30 )
    {
        // lastHitPoint = hitPoint == Vector3.zero ? enemyCollider.ClosestPoint(t_target.position) : hitPoint;      // 플레이어와 적 개체의 콜라이더가 겹쳐있는 경우, hitPoint 가 (0,0,0)이 나옴;
        lastHitPoint = hitPoint == Vector3.zero ? myTransform.position : hitPoint;
        // Debug.Log(lastHitPoint);
    
        GetDamaged(damage,impulse);
    }

    void GetDamaged(float damage, float impulse)
    {
        // cc기 면역인 경우엔 넉백 안받음. 
        if(status.ccImmunity==false && status.tenacity < impulse)
        {
            GetKnockback(impulse, lastHitPoint);
        }
        

        //
        status.currHp -= damage;
        OnHit();
        if (isAlive == false)
        {
            Die();
        }

        // 데미지 텍스트 생성
        DamageType damageType = impulse >30  ? DamageType.DMG_CRITICAL : DamageType.DMG_NORMAL;  // 이건 추후에 크리정보 담아서, 
        PoolManager.Instance.effectPoolSys.GetDamageText(damage,lastHitPoint,damageType) ;
        PoolManager.Instance.effectPoolSys.GetEnemyHitEffect(lastHitPoint);
        // sfx
        SoundManager.Instance.Invoke(myTransform, SoundEventType.Enemy_Hit);
    }

    public void GetHealed(float heal)
    {
        //
        if (heal <=0)
        {
            return;
        }
        
        status.currHp += heal;
    }




    //=========================================================================================

    // knockBack 
    public void GetKnockback(float impulse, Vector3 hitPoint)
    {
        statusEffects.GetNockbacked(t_target.position,hitPoint,impulse, 0.5f);
        OnStunned();
    }



    void OnStunned()
    {
        animationController.OnStunned();
        abilitySystem.Interrupt();
    }


    void Die()
    {
        //
        DropItem();
        SoundManager.Instance.Play(myTransform, data.sfx_die);

        // 사망시 공통처리
        DeathSequence().Forget(); 
    }

    public void CleanDeath()
    {
        if (isAlive == false)
        {
            return;
        }
  
        // GetKnockback(20, myTransform.position);

        // 사망시 공통처리
        DeathSequence().Forget(); 
    }

    void DropItem()
    {
        PoolManager.Instance.dropItemPoolSys.GetExp( data.exp, myTransform.position);
        
        // if (BW.Math.GetRandom(0, 100) < 50)
        // {
        //     PoolManager.Instance.dropItemPoolSys.GetHpUp(30, myTransform.position);
        // }
    }




    public void OnDie()
    {
        abilitySystem.OnDie();
    }

    //==================================================
    /// <summary>
    /// 적 사망 애니메이션을 재생하고, 해당 애니메이션이 종료후 오브젝트를 제거한다. 
    /// </summary>
    async UniTask DeathSequence()
    {
        activated  = false;
        enemyCollider.enabled = false; // 적 탐색 및 총알 충돌에 걸리지 않도록.
        OnDie();
        OnDie_Custom();

        await animationController.WaitForDeathAnimation();
        
        PoolManager.Instance.enemyPoolSys.Return(this);
    }
}