using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System;

#region 피격 후 무적
public class SE_P00_InvincibleOnHit : PlayerStatusEffect
{
    public float impulse;
    public float dir;
    public Vector3 targetVelocity;

    public Sequence seq;
    

    public SE_P00_InvincibleOnHit()
    {
        Init_Custom();
    }
    //============================================================================
    protected override void Init_Custom()
    {
        this.target = Player.Instance;
        // this.data = ResourceManager.Instance.statusEffectDic.GetData("P00") as PlayerStatusEffectSO;
        this.cached = true;
    }


    protected override void OnEnter_Custom()
    {
        target.status.stack_invincible ++;
    }

    protected override void OnExit_Custom()
    {
        target.status.stack_invincible--;
    }

    protected override void OnUpdate_Custom(float currDuration)
    {
        
    }

    //===========================================================================
}
#endregion


#region 넉백
public class SE_P01_Nockback : PlayerStatusEffect
{
    public float impulse;
    public Vector3 dir;
    public Vector3 targetVelocity;

    public Sequence seq;

    public SE_P01_Nockback()
    {
        Init_Custom();
    }
    
    //============================================================================
    protected override void Init_Custom()
    {
        this.target = Player.Instance;
        // this.data = ResourceManager.Instance.statusEffectDic.GetData("P01") as PlayerStatusEffectSO;
        this.cached = true;
    }


    protected override void OnEnter_Custom()
    {
        target.status.forcedMoveVelocity = targetVelocity;

        // forcedMoveVelocity를 (0,0,0)로 2초 동안 줄이기
        if( seq!=null && seq.IsActive())
        {
            seq.Kill();
        }
        seq = DOTween.Sequence()
        .Append( 
            DOTween.To(
                () => target.status.forcedMoveVelocity,     // 현재 Vector3 값을 반환
                x => target.status.forcedMoveVelocity = x,  // 변경된 값을 대입
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
        target.status.forcedMoveVelocity = Vector3.zero;
        // Debug.Log("넉백종료");
    }

    protected override void OnUpdate_Custom(float currDuration)
    {
        // Debug.Log($"{currDuration}/{totalDuration}");
    }

    //===========================================================================
}

#endregion


#region 스턴
public class SE_P02_Stun : PlayerStatusEffect
{

    public SE_P02_Stun()
    {
        Init_Custom();
    }
    
    //============================================================================
    protected override void Init_Custom()
    {
        this.target = Player.Instance;
        // this.data = ResourceManager.Instance.statusEffectDic.GetData("P02") as PlayerStatusEffectSO;
        this.cached = true;
    }


    protected override void OnEnter_Custom()
    {
        target.status.stack_immobilized ++;
        target.status.stack_disarmed ++;
        target.status.stack_uncastable ++;
    }

    protected override void OnExit_Custom()
    {
        target.status.stack_immobilized --;
        target.status.stack_disarmed --;
        target.status.stack_uncastable --;
        // Debug.Log("넉백종료");
    }

    protected override void OnUpdate_Custom(float currDuration)
    {
        // Debug.Log($"{currDuration}/{totalDuration}");
    }

    //===========================================================================
}

#endregion

#region 슬로우
public class SE_P03_Slow : PlayerStatusEffect
{
    public float lastSlowAmount;

    Queue<float> applyedSlowAmount = new();

    public SE_P03_Slow()
    {
        Init_Custom();
    }
    
    //============================================================================
    protected override void Init_Custom()
    {
        this.target = Player.Instance;
        this.cached = true;
    }


    protected override void OnEnter_Custom()
    {
        applyedSlowAmount.Enqueue(lastSlowAmount);
        float slowAmount = applyedSlowAmount.Last();
        target.status.movementSpeedMultiplier.value -= slowAmount;
    }

    protected override void OnExit_Custom()
    {
        float slowAmount = applyedSlowAmount.Dequeue();
        target.status.movementSpeedMultiplier.value += slowAmount;
        // Debug.Log("넉백종료");
    }

    protected override void OnUpdate_Custom(float currDuration)
    {
        // Debug.Log($"{currDuration}/{totalDuration}");
    }

    //===========================================================================
}

#endregion



