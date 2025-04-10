using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashSystem : MonoBehaviour
{
    PA_4000_DashBase data;
    
    



    public void Init(PA_4000_DashBase data)
    {
        this.data= data;
    }


    // public void ActivateDash()
    // {
    //     StartCoroutine( DashRoutine() );
    // }


    // protected IEnumerator DashRoutine( int currDashCount)
    // { 
    //     Player player = Player.Instance;
        
    //     if (currDashCount> maxDashCount )
    //     {
    //         yield break;
    //     }
        
    //     SoundManager.Instance.Invoke(Player.Instance.t, SoundEventType.Player_Dash);        // 나중에 사운드 파일 별도로 저장. 
    //     bool ComboDashDetected = false;
        

    //     Vector3 dir = player.lastMoveDir.normalized;
        
    //     // 대시 실행  
    //     Vector3 forcedMoveVelocity = dir * player.status.movementSpeed * dashMultiplier;
    //     player.status.forcedMoveVelocity += forcedMoveVelocity;
    //     player.status.stack_immobilized++;
    //     player.status.stack_invincible++;

    //     // 대시 지속
    //     float totalElapsedTime = 0f;
    //     float elapsedTime = 0f;
    //     float comboDetectionStartTime = (dashDuration + delay_afterCast) * comboDetectionThreshold;
    //     while (elapsedTime < dashDuration)
    //     {
            
    //         if( totalElapsedTime >= comboDetectionStartTime && PlayerInputManager.Instance.util== true )
    //         {
    //             ComboDashDetected = true;
    //         }

    //         elapsedTime += Time.unscaledDeltaTime;
    //         totalElapsedTime  += Time.unscaledDeltaTime;
    //         yield return null;
    //     }

    //     // 원래 속도로 복구
    //     player.status.forcedMoveVelocity -= forcedMoveVelocity;
    //     player.status.stack_immobilized--;

    //     elapsedTime = 0f;
    //     while (elapsedTime < delay_afterCast)
    //     {
            
    //         if( totalElapsedTime  >= comboDetectionStartTime &&  PlayerInputManager.Instance.util== true )
    //         {
    //             ComboDashDetected = true;
    //             break;
    //         }
    //         elapsedTime += Time.unscaledDeltaTime;
    //         totalElapsedTime +=Time.unscaledDeltaTime;
    //         yield return null;
    //     }
    //     player.status.stack_invincible--;

    //     // 연속 대쉬 
    //     if ( ComboDashDetected  && currDashCount < maxDashCount )
    //     {
    //         player.StartCoroutine(DashRoutine(player,1));
    //         player.StartCoroutine(DashRoutine_Custom(player));       
    //     }
    // }




    //===================

    // /// <summary>
    // /// 직접 충돌 시 적에게 피해를 입힌다. (성능 테스트 필요)
    // /// </summary>
    // /// <param name="player"></param>
    // /// <returns></returns>
    // protected IEnumerator BodyAttackRoutine(Player player)
    // {
    //     float baseDamage = data.baseDamage;
    //     float coefficient_pDmg = data.coefficient_pDmg;
        
        
    //     float damage = player.status.GetFinalPDmg(baseDamage, coefficient_pDmg);

    //     Dictionary<Enemy,bool> collidedEnemies = new();


    //     float elapsedTime = 0;
    //     while(elapsedTime < dashDuration )
    //     {
    //         Collider[] hits = Physics.OverlapSphere(player.t.position.WithFloorHeight(), damageRadius, GameConstants.enemyLayer);

    //         // 충돌지역에 플레이어가 있으면. 
    //         if(hits.Length>0)
    //         {
    //             foreach(Collider hit in hits)
    //             {
    //                 // 적에게 피해를 입히는 로직
    //                 Enemy enemy = hit.GetComponent<Enemy>();
    //                 if (enemy != null && collidedEnemies.ContainsKey( enemy )==false)
    //                 {
    //                     collidedEnemies[enemy] = true;
    //                     enemy.GetDamaged( hit.ClosestPoint( player.t.position),damage );
    //                 }
    //             }

    //         }
            
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }
    // }

}
