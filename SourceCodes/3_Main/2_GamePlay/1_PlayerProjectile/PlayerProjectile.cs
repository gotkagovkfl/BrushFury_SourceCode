using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public abstract class PlayerProjectile : BwPoolObject
{
    public override PoolType poolType => PoolType.PlayerProjectile; 
    public SkillItemSO baseData;

    //
    [SerializeField] protected Transform t_follow;     // 움직임을 따라다닐 대상 ( 주로 플레이어 )
    [SerializeField] protected Vector3 followOffset;   // 따라다닐 대상으로부터 떨어질 거리. 
    protected Coroutine followRoutine;
    //
    [SerializeField] protected Transform t_target;     // 유도 투사체의 경우. 
    protected Vector3 targetPos;
    protected Vector3 dir;        // 투사체 방향
    
    public bool isAlive;


    // [SerializeField] protected float defaultDamage;
    // [SerializeField] protected float damageWeight;
    

    public float finalDmg;
    //=================================================


    //===================================================
    public void Init(float dmg, Vector3 targetPos)
    {
        isAlive = true;
        finalDmg = dmg;
        this.targetPos = targetPos;
        this.dir = (targetPos - myTransform.position).normalized;
        Init_Custom();
    }

    public void Init(SkillItemSO skillData, Vector3 targetPos)
    {
        baseData = skillData;
        finalDmg = Player.Instance.status.GetFinalPDmg(skillData.baseDamage, skillData.coefficient_pDmg);
        Init(finalDmg, targetPos);
    }

    protected abstract void Init_Custom();
    
    //====================================================
    public void Follow(Transform t_follow, Vector3 followOffset  = default)
    {
        this.t_follow = t_follow;
        this.followOffset = followOffset;
        
        // PoolManager의 자식으로 있어야하기 때문에, SetParent로 할 수가 없음; -> 되네?
        myTransform.SetParent(t_follow);
        myTransform.localPosition = followOffset;

        // if ( followRoutine !=null)
        // {
        //     StopCoroutine( followRoutine );
        // }
        // followRoutine =StartCoroutine(FollowRoutine());
    }

    IEnumerator FollowRoutine()
    {
        while( isAlive )
        {
            myTransform.position = t_follow.position + followOffset;
            yield return null;
        }
    }
    //=========================================================
    public void DestroyProjectile()
    {
        if (isAlive ==false)
        {
            return;
        }
        
        isAlive = false;
        StopAllCoroutines();
        //풀링
        PoolManager.Instance.playerProjectilePoolSys.Return(this);
    }

    protected IEnumerator DestroyRoutine(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        DestroyProjectile();
    }


    public override void OnCreatedInPool()
    {
        
    }

    public override void OnGettingFromPool()
    {
        
    }
}
