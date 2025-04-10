using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

#region 스턴
public class SE_00_Stun : EntityStatusEffect
{
    public float impulse;
    public float dir;
    public Vector3 targetVelocity;

    public Sequence seq;
    

    public SE_00_Stun(EntityStatus targetStatus) : base()
    {
        Init(targetStatus);
    }
    //============================================================================
    protected override void Init_Custom()
    {
        // this.data = ResourceManager.Instance.statusEffectDic.GetData("P00") as PlayerStatusEffectSO;
        this.cached = true;
    }


    protected override void OnEnter_Custom()
    {
        targetStatus.stack_immobilized ++;
        targetStatus.stack_disarmed ++;
        targetStatus.stack_uncastable ++;
    }

    protected override void OnExit_Custom()
    {
        targetStatus.stack_immobilized --;
        targetStatus.stack_disarmed --;
        targetStatus.stack_uncastable --;
    }

    protected override void OnUpdate_Custom(float currDuration)
    {
        
    }


}
#endregion

    //===========================================================================================================================================
#region 넉백
public class SE_01_Nockback : EntityStatusEffect
{
    public float impulse;
    public Vector3 dir;
    public Vector3 targetVelocity;

    public Sequence seq;

    public SE_01_Nockback(EntityStatus targetStatus) : base()
    {
        Init(targetStatus);
    }
    
    //============================================================================
    protected override void Init_Custom()
    {
        // this.target = Player.Instance;
        // this.data = ResourceManager.Instance.statusEffectDic.GetData("P01") as PlayerStatusEffectSO;
        this.cached = true;
    }


    protected override void OnEnter_Custom()
    {
        targetStatus.forcedMoveVelocity = targetVelocity;
        // Debug.Log("아니!!");
        // forcedMoveVelocity를 (0,0,0)로 2초 동안 줄이기
        if( seq!=null && seq.IsActive())
        {
            seq.Kill();
        }
        seq = DOTween.Sequence()
        .Append( 
            DOTween.To(
                () => targetStatus.forcedMoveVelocity,     // 현재 Vector3 값을 반환
                x => targetStatus.forcedMoveVelocity = x,  // 변경된 값을 대입
                Vector3.zero,                               // 목표 값 (0,0,0)
                totalDuration                           // 소요 시간 (초)
            )
        )
        .SetEase(Ease.OutCirc)
        .Play();
        // .SetEase(Ease.OutQuad); // 이징 함수 설정 (원하는 방식으로 변경 가능)

    }

    protected override void OnExit_Custom()
    {
        targetStatus.forcedMoveVelocity = Vector3.zero;
        // Debug.Log("넉백종료");
    }

    protected override void OnUpdate_Custom(float currDuration)
    {
        // Debug.Log($"{currDuration}/{totalDuration}");
    }

    
}

#endregion

//===========================================================================================================================================

#region 슬로우 

public class SE_02_Slow
{

}


#endregion




