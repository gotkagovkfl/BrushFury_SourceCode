using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BW;

public class EnemyStatusEffects : MonoBehaviour
{
    Enemy enemy;
    
    [Header("Default SE")]
    [SerializeField] SE_00_Stun se_stunned;
    [SerializeField] SE_01_Nockback se_nockbacked;
    
    
    [Header("SE")]
    public List<EntityStatusEffect> statusEffects;


    //=====================================================================
    public void Init(Enemy enemy)
    {
        this.enemy = enemy;
        
        se_stunned = new(enemy.status);
        se_nockbacked = new(enemy.status);
        
        statusEffects = new(){se_stunned, se_nockbacked };
        // 무적, 스턴 미리 넣어두기. 
        // Debug.Log(statusEffects.Count); 
    } 

    public void OnUpdate()
    {
        // 현재 플레이어의 상태이상을 갱신한다. 
        for(int i= statusEffects.Count-1;i>=0;i--)
        {
            EntityStatusEffect se = statusEffects[i];
            se.OnUpdate();

            // 만료된 상태이상 삭제 - 캐시 제외 
            if (se.activated == false && se.cached==false)
            {
                statusEffects.Remove( se );
            }
        }

    }

    //====================================================================================================================

    // public void EnterStatusEffect(PlayerStatusEffectSO statusEffectData)
    // {
        
    //     PlayerStatusEffect se = new(Player.Instance, statusEffectData, false);
    // }

    // public void SetInvincibleOnHit(float targetDuration)
    // {
    //     // 무적 ++
    //     se_invincible.OnEnter(targetDuration);
    // }

    
    void SetNockbacked(float targetDuration, Vector3 targetVelocity)
    {
        // se_nockbacked.dir = dir;
        // se_nockbacked.impulse = impulse;
        se_nockbacked.targetVelocity = targetVelocity;
        se_nockbacked.OnEnter(targetDuration);
    }   

    public void SetStunned(float targetDuration)
    {
        //이동불가, 공격불가, 능력사용불가,
        se_stunned.OnEnter(targetDuration);
    }

    //======================================================================

    public void GetNockbacked(Vector3 attackerPos, Vector3 hitPoint, float impulse, float targetDuration)
    {
        Vector3 targetPos =  enemy.myTransform.position;
        Vector3 dir = targetPos - hitPoint;
        if (dir == Vector3.zero)
        {
            dir = targetPos - attackerPos;
        }
        dir = dir.WithFloorHeight().normalized;
        
        //이동불가, 공격불가, 능력사용불가, + 강제이동
        Vector3 targetVelocity = impulse* dir;
        // Debug.Log($"{impulse} , {targetVelocity} ");
        SetStunned(targetDuration );
        SetNockbacked(targetDuration * 0.8f, targetVelocity );
    }
}
