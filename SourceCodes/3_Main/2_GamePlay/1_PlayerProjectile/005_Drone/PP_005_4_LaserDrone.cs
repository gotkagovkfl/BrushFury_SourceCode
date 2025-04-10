using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BW;

public class PP_005_4_LaserDrone : PP_005_0_Drone 
{
    protected override string id => "005_4";


    protected PlayerProjectile prefab_laser;
    float duration_laser;


    //=================================================================

    protected override void Init_Custom()
    {
        currState = State.Ready;

        // Debug.Log($"{baseData} / {baseData is PA_005_Drone}");
        if( baseData is PA_005T_LaserDrone droneData)
        {
            cost = droneData.cost_perRoutine;
            routineInterval = droneData.interval_perRoutine;
            attackCount = droneData.attackCount_perRoutine;  
            attackInterval = droneData.interval_perAttack;  
            attackRange = droneData.attackRange_drone;
            cep = droneData.cep;
            prefab_projectile = droneData.prefab_pp_droneProjectile;   


            //
            prefab_laser = droneData.pp_laser;
            duration_laser = droneData.duration_laser;
        }
    }


    public override IEnumerator AttackRoutine()
    {
        WaitForSeconds wfs_routine = new(routineInterval);
        WaitForSeconds wfs_attack = new(attackInterval);

        while( currState != State.Deactivated)
        {
            // 공격루틴 한번
            // Debug.Log(attackCount);
            Vector3 targetPos = GetTargetPos(1)[0];
            for(int i=0;i<attackCount;i++)
            {
                Vector3 fixedPos = targetPos += Math.GetRandomPointOnCircle(cep);   // 위치 보정
                // 공격
                var pp_droneProj = PoolManager.Instance.playerProjectilePoolSys.GetPP(baseData, prefab_projectile, myTransform.position, fixedPos);
                yield return wfs_attack;
            }
            yield return LaserRoutine(targetPos);   //이게 실행됨. 
            // 다음 루틴 기다리기 
            yield return wfs_routine;

            // 다음 루틴 진행 검사
            if( Player.Instance.status.currInk >=cost)
            {
                currState = State.Activated;
                Player.Instance.UseInk(cost);
            }
            else
            {
                currState = State.Unavailable;
                yield return new WaitUntil(()=>Player.Instance.status.currInk >=cost);
            }

        }
    }

    IEnumerator LaserRoutine(Vector3 targetPos)
    {
        var laser = PoolManager.Instance.playerProjectilePoolSys.GetPP<PP_005_2_DroneLaser>(baseData, prefab_laser, myTransform.position, targetPos);   
        laser.drone = this; 
        yield return new WaitForSeconds(duration_laser);
    }

}
