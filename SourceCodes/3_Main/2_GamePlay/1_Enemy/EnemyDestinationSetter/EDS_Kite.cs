using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EDS_Kite : EDS_EncirclingApproach
{
    protected float retreatRange => enemy.status.retreatRange;


    bool retreatPosUpdated;



    public EDS_Kite(Enemy enemy) : base(enemy)
    {
        currDest = enemy.myTransform.position;
    }


    protected override void Update_Custom(float currAbilityRange)
    {
        float targetDistSqr = enemy.targetDistSqr;
        if (targetDistSqr <= retreatRange * retreatRange)   // 후퇴거리 안 일때, 
        {
            if ( retreatPosUpdated  == false || destDistSqr < 2f)
            {
                SetNewDest();
                retreatPosUpdated = true;
            }

        }
        else
        {
            // 
            if (currAbilityRange<0)
            {
                currAbilityRange = GetOtherAbilityRange();  // 보유 능력들의 가장 짧은 사거리를 얻는다. (가만히 서있기 위함)
            }

            // 공격 사거리 안에 있을 땐 움직이지 않음. 
            if( targetDistSqr <= currAbilityRange *currAbilityRange )
            {
                // do nothing
            }
            // 근데 밖이라면 사거리 안으로 이동. 
            else
            {
                base.Update_Custom(currAbilityRange);
            }
            retreatPosUpdated = false;
        }  
    }


    void SetNewDest()
    {
        Vector3 targetPos = enemy.t_target.position;
        currDest = enemy.myTransform.position + (enemy.myTransform.position - targetPos).normalized * 2f;
        

        // 첫번째 dest 가 navMeshSurface 위의 좌표가 아니라면, 플레이어 근처의 랜덤 좌표를 dest 로 지정한다. 
        if (NavMesh.SamplePosition(currDest , out NavMeshHit hit, 0.5f, NavMesh.AllAreas) == false)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = targetPos + Random.insideUnitSphere * 5f;
                NavMeshHit hit2;
                if (NavMesh.SamplePosition(randomPoint, out hit2, 0.5f, NavMesh.AllAreas))
                {
                    currDest  = hit2.position;

                    break;
                }
            }
        }

    }

    float GetOtherAbilityRange()
    {
        float ret = 0;

        ret = enemy.data.abilities.Min(x=>x.range);
        return ret;
    }
}
