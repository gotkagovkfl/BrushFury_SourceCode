using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EDS_EncirclingApproach : EnemyDestinationSetter  
{
 // [Header(" Dest Offset")]
    protected float initDestOffseCooltime = 2f;
    protected float lastInitDestOffestTime = -3f;
    protected  Vector3 destOffset;        // 자연스러운 이동처럼 보이기 위한 타겟 오프셋

    protected float destOffsetRadius_first = 15;
    protected float destOffsetRadius_second = 6;
    


    
    //==============================================================
    public EDS_EncirclingApproach(Enemy enemy) : base(enemy)
    {

    }

    protected override void Update_Custom(float currAbilityRange)
    {
        TryUpdateDestOffest();

        currDest = enemy.t_target.position + destOffset;
    }

    protected void TryUpdateDestOffest()
    {
        if(Time.time < lastInitDestOffestTime  + initDestOffseCooltime )
        {
            return;
        }
        lastInitDestOffestTime = Time.time;
        
        // first 반경 위의 점
        if ( enemy.targetDistSqr >   destOffsetRadius_first *  destOffsetRadius_first * 1.1f)
        {
            // 3D XZ 평면으로 매핑 후, 원하는 반경과 중심 적용
            destOffset =   BW.Math.GetPointOnUnitCircleCircumference(destOffsetRadius_first);

        }
        // second 반경위의 점
        else if ( enemy.targetDistSqr > destOffsetRadius_second *  destOffsetRadius_second * 1.1f )
        {
            destOffset =   BW.Math.GetPointOnUnitCircleCircumference(destOffsetRadius_second);
        }
        // 직선 접근
        else
        {
            destOffset = Vector3.zero;
        }
    }
}
