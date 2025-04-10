using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]

public class EnemyStatus : EntityStatus
{
    //
    #region 기본값
    // [SerializeField] protected float d_range;
    [SerializeField] protected float d_retreatRange;
    [SerializeField] protected float d_tenacity;    // 기절 저항력 - 충격량보다 높으면 기절하지 않는다 ( 넉백 및 스턴 )
    #endregion
    

    //
    #region 수정값
    public  float rangeWeight; 
    public float tenacityModifier;
    #endregion




    //
    #region 최종값

    public float collisionDmg => pDmg ;
    // public float range => d_range * rangeWeight;
    public float retreatRange => d_retreatRange * rangeWeight;

    public Vector3 targetPosition;  // 상태이상이 아닐 때 목표로 하는 포지션.

    // public EnemyRank rank;
    public float tenacity => d_tenacity + tenacityModifier;
    #endregion


    #region 상태이상
    public bool canKnockbackPlayer => stack_canKnockbackPlayer>0;
    public bool ccImmunity => stack_ccImmunity>0; 

    
    public int stack_canKnockbackPlayer;

    public int stack_ccImmunity; 



    #endregion

    //=================================================================

    public EnemyStatus()
    {

    }

    public EnemyStatus(EnemyDataSO eData)
    {
        d_maxHp = eData.maxHp;
        d_movementSpeed = eData.movementSpeed;
        pDmg  = eData.ad;
        // mDmg = eData.ap;
        

        // d_range = eData.range;
        d_retreatRange = eData.retreatRange;
        
        rangeWeight = BW.Math.GetRandom(0.75f, 1.25f);

        d_tenacity = eData.tenacity;
        // rank = eData.rank;


        //==============

        currHp = maxHp;


        //
        stack_canKnockbackPlayer =0;
        stack_disarmed = 0;
        stack_immobilized = 0;
        stack_uncastable = 0;
        stack_invincible = 0;
        stack_ccImmunity = 0;
    }

}
