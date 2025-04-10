using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;

/// <summary>
/// 플레이어의 상태 이상 정보
/// </summary>
public class PlayerStatusEffects : MonoBehaviour
{
    [Header("Default SE")]
    [SerializeField] SE_P00_InvincibleOnHit se_invincible;
    [SerializeField] SE_P01_Nockback se_nockbacked;
    [SerializeField] SE_P02_Stun se_stunned;
    [SerializeField] SE_P03_Slow se_slow;
    
    [Header("SE")]
    public List<PlayerStatusEffect> statusEffects;


    //=====================================================================
    public void Init()
    {
        se_invincible = new();
        se_stunned = new();
        se_nockbacked = new();
        se_slow = new();
        
        statusEffects = new(){se_invincible,se_stunned, se_nockbacked, se_slow  };
        // 무적, 스턴 미리 넣어두기. 
        // Debug.Log(statusEffects.Count); 
    } 

    public void OnUpdate()
    {
        // 현재 플레이어의 상태이상을 갱신한다. 
        for(int i= statusEffects.Count-1;i>=0;i--)
        {
            PlayerStatusEffect se = statusEffects[i];
            se.OnUpdate();

            // 만료된 상태이상 삭제 - 캐시 제외 
            if (se.activated == false && se.cached==false)
            {
                statusEffects.Remove( se );
            }
        }

    }

    // public void EnterStatusEffect(PlayerStatusEffectSO statusEffectData)
    // {
        
    //     PlayerStatusEffect se = new(Player.Instance, statusEffectData, false);
    // }

    public void SetInvincibleOnHit(float targetDuration)
    {
        // 무적 ++
        se_invincible.OnEnter(targetDuration);
    }

    
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

    public void GetSlow(float amount,float targetDuration)
    {
        se_slow.lastSlowAmount = amount;
        se_slow.OnEnter(targetDuration);
    }

    //======================================================================

    public void GetNockbacked(Vector3 enemyPos, Vector3 hitPoint, float impulse)
    {
        Vector3 targetPos =  Player.Instance.t.position;
        Vector3 dir = targetPos - hitPoint;
        if (dir == Vector3.zero)
        {
            dir = targetPos - enemyPos;
        }
        dir = dir.WithFloorHeight().normalized;
        
        //이동불가, 공격불가, 능력사용불가, + 강제이동
        Vector3 targetVelocity = impulse* dir;
        // Debug.Log($"{impulse} , {targetVelocity} ");
        float targetDuration = 1f;
        SetStunned(targetDuration );
        SetNockbacked(targetDuration * 0.8f, targetVelocity );
    }
}
