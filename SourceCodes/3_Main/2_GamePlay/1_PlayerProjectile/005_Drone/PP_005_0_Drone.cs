using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BW;
using System.Resources;


public class PP_005_0_Drone : PlayerProjectile
{
    protected override string id => "005_0";
    
    
    
    
    public enum State
    {
        Ready,
        Activated,      // 활성화됨 ( 공격 중 )
        Unavailable,    // 마나부족상태 
        Deactivated,    // 비활성화됨. 
    }
    public State currState;
    





    protected float targetDist = 5f;
    
    
    protected float cost;
    protected float routineInterval;

    protected int attackCount;       
    protected float attackInterval;
    protected float attackRange;
    protected float cep;

    protected PlayerProjectile prefab_projectile;

    protected override void Init_Custom()
    {
        currState = State.Ready;

        // Debug.Log($"{baseData} / {baseData is PA_005_Drone}");
        if( baseData is PA_005_Drone droneData)
        {
            cost = droneData.cost_perRoutine;
            routineInterval = droneData.interval_perRoutine;
            attackCount = droneData.attackCount_perRoutine;  
            attackInterval = droneData.interval_perAttack;  
            attackRange = droneData.attackRange_drone;
            cep = droneData.cep;
            prefab_projectile = droneData.prefab_pp_droneProjectile;   
        }
    }

    public IEnumerator ActivationRoutine(float duration)
    {
        
        float elapsed = 0;
        while(elapsed <= duration)
        {
            float ratio = elapsed / duration;

            
            Vector3 temp = followOffset * targetDist * ratio;
            Vector3 targetPosition = new Vector3(temp.x, 1, temp.z);    

            myTransform.localPosition = targetPosition;
            //
            elapsed += Time.fixedDeltaTime;
            yield return GameConstants.waitForFixedUpdate;
        }
        currState = State.Activated;
    }

    public virtual IEnumerator AttackRoutine()
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



    public IEnumerator DeactivationRoutine(float duration)
    { 
        float elapsed = 0f;
        while ( elapsed <= duration)
        {
            float ratio =  1 - elapsed / duration;
            
            Vector3 temp = followOffset * targetDist * ratio;
            Vector3 targetPosition = new Vector3(temp.x, 1, temp.z);    

            myTransform.localPosition = targetPosition;

            //
            elapsed += Time.fixedDeltaTime;
            yield return GameConstants.waitForFixedUpdate;
        }
        currState = State.Deactivated;

        PoolManager.Instance.playerProjectilePoolSys.Return( this );
    }   






    //==========================================================================

    /// <summary>
    /// 드론의 공격 위치를 계산한다. - 레이저 쓸땐 targetCount 를 2로해서 쓰기. 
    /// </summary>
    /// <param name="targetCount"></param>
    /// <returns></returns>
    protected List<Vector3> GetTargetPos(int targetCount)
    {
        List<Vector3> ret = new();

        Collider[] hits = Physics.OverlapSphere(myTransform.position.WithFloorHeight(), attackRange, GameConstants.enemyLayer);
        
        
        int currIdx = 0;
        for(int i=0;i<targetCount;i++)
        {
            Vector3 targetPos = myTransform.position.WithFloorHeight();
            // 범위 안에 적이 있는 경우, 아무 적의 위치 
            if (currIdx < hits.Length )
            {
                targetPos = hits[currIdx].transform.position;
            }
            // 범위 내에 적이 없는 경우. 범위 내 아무 지점. 
            else
            {
                targetPos += Math.GetRandomPointOnCircle(attackRange);
            }

            currIdx++;
            ret.Add(targetPos);
        }

        return ret;
    }



    // 범위 표시 
    private void OnDrawGizmos()
    {
        if ( myTransform ==null)
        {
            return;
        }
        
        Gizmos.color = Color.yellow; // 초록색 원
        Gizmos.DrawWireSphere(myTransform.position.WithFloorHeight(), attackRange);


    }
}
