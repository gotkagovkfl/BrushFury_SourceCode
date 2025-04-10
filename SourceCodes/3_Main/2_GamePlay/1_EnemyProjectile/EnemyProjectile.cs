using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using BW;


public abstract class EnemyProjectile : BwPoolObject
{
    protected Enemy enemy;
    protected EnemyAbilitySO eaData;

    public float damage;


    Coroutine destroyRoutine;

    public override PoolType poolType => PoolType.EnemyProjectile;
    protected bool activated; 

    // protected Vector3 initPos;
    protected Vector3 targetPos;
    protected Vector3 dir;


    //=======================================================

    /// <summary>
    /// 투사체 초기화
    /// </summary>
    /// <param name="abilityData"></param> 크기 및 스프라이트 설정
    /// <param name="enemy"></param>      데미지 설정
    /// <param name="initPos"></param>     초기위치
    /// <param name="lifeTime"></param>     수명
    public void Init( Enemy enemy, EnemyAbilitySO eaData, Vector3 initPos, Vector3 targetPos)
    {
        this.enemy = enemy;
        this.eaData = eaData;
        damage = eaData.AbilityDmg(enemy);
        this.targetPos = targetPos;
        
        dir = (targetPos - initPos).WithFloorHeight().normalized; 
        if( dir != Vector3.zero)
        {
            myTransform.rotation = Quaternion.LookRotation(dir);
        }
        

        Init_Custom();
        activated = true;

        //
        DestroyProjectile();
    }

    protected abstract void Init_Custom();

    //================================================

    public override void OnCreatedInPool()
    {
        // projCollider = GetComponent<Collider>();
        // rb = GetComponent<Rigidbody>();
    }

    public override void OnGettingFromPool()
    {
        // projCollider.enabled = true;
    }

    //================================================

    public void DestroyProjectile()
    {
        if(destroyRoutine!=null)
        {
            StopCoroutine(destroyRoutine);
        }
        destroyRoutine = StartCoroutine( DestroyRoutine());
    }
    
    IEnumerator DestroyRoutine()
    {
        yield return DestroyCondition();
        activated = false;
        PoolManager.Instance.enemyProjectilePoolSys.Return(this);
    }

    public void DestroyImmediately()
    {
        if(destroyRoutine!=null)
        {
            StopCoroutine(destroyRoutine);
        }
        activated = false;
        PoolManager.Instance.enemyProjectilePoolSys.Return(this);
    }

    protected abstract IEnumerator DestroyCondition();
}
