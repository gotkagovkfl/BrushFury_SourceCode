using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;


using BW;


/// <summary>
/// AI보단 movement 느낌. 
/// </summary>
[RequireComponent(typeof(NavMeshAgent)) ]

public class EnemyAI : MonoBehaviour
{
    
    EnemyDestinationSetter  destSetter;
    // public EnemyAbilitySystem abilitySystem;
    // EnemyFSM fsm;
    public NavMeshAgent navAgent;
    
    Enemy enemy;

    

    //==========================================


    public float retreatRange => enemy.status.retreatRange;
    public float reationRate = 0.2f;
    float lastAiUpdateTime = -2f;
    bool canUpdateAI => Time.time >= lastAiUpdateTime + reationRate; 

    public bool canMove => destSetter !=null && enemy.status.immobilized==false ;



    //===========================================================================
    public void Init(Enemy enemy)
    {
        this.enemy = enemy;

        navAgent = GetComponent<NavMeshAgent>();
        navAgent.isStopped = false;
        navAgent.autoBraking = false;
        navAgent.speed = enemy.status.movementSpeed;
        navAgent.stoppingDistance = 0;
        // navAgent.angularSpeed = 10000f; 

        InitDestSetter(enemy.data.moveType);
    }

    public void OnActiavted()
    {
        
    }


    void InitDestSetter(EnemyMoveType moveType)
    {
        switch (moveType)
        {
            case EnemyMoveType.SimpleApproach:
                destSetter = new EDS_SimpleApproach(enemy);
                break;
            
            case EnemyMoveType.EncirclingApproach:
                destSetter = new EDS_EncirclingApproach(enemy);
                break;
                
            case EnemyMoveType.Kite:
                destSetter = new EDS_Kite(enemy);
                break;
        }
    }

    //=============================================
    void Update()
    {
        if (GamePlayManager.isGamePlaying == false)
        {
            navAgent.isStopped = true;
        }
        else
        {
            navAgent.isStopped = false;
        }

        Move();
    }


    public void OnUpdate(EnemyAbilitySystem abilitySystem)
    {
        if( canMove )
        {
            float currAbilityRange = abilitySystem.usingAbility!=null? abilitySystem.currAbilityRange:-1;
            destSetter?.TryUpdate(currAbilityRange); // destSetter가 없으면 이동하지 않음.
        }
    }




    //===========================================================





    // private void OnDrawGizmos()
    // {
    //     if( navAgent==null)
    //     {
    //         return;
    //     }
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawSphere(navAgent.destination, 0.3f);

    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(enemy.status.targetPosition, 0.3f);

    // }






    /// <summary>
    /// 움직임
    /// </summary>
    public void Move()
    {
        MoveActively();
        MovePassively();
    }


    
    
    void MoveActively()
    {
        // 이동불가 상태의 경우, 실행 x - 유저의 입력을 받아 이동
        if (canMove==false || enemy.isAlive==false )
        {
            navAgent.velocity = Vector3.zero;
            return;
        }

        float targetSpeed = enemy.status.movementSpeed;
        navAgent.speed = targetSpeed;
        navAgent.velocity = navAgent.desiredVelocity; 

        //
        Vector3 targetPosition = enemy.status.targetPosition;
        navAgent.SetDestination( targetPosition );   
    }

    /// <summary>
    /// 상태 이상 걸렸을 때를 위함. - 넉백, 공포, 매혹 등이 있을 듯. 
    /// </summary>
    void MovePassively()
    {
        // 이동불가 상태의 경우에만 실행 
        Vector3 moveVector =  enemy.status.forcedMoveVelocity; // 넉백 벡터 추가
        if(moveVector.magnitude<0.1f)
        {
            return;
        }

        navAgent.velocity = moveVector;
    }



    //============================================================================================


    //================================



    //==================================
}
