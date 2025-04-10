using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyStage : Stage
{

    public override IEnumerator StageStartSequence()
    {
        return null;
    }

    protected override void FinishStage_Custom()
    {
        
    }

    protected override void Init_Custom()
    {
        Debug.Log("빈 스테이지 초기화 ");
    }

    protected override void StartStage_Custom()
    {
        
    }

    //     public override bool IsStageFinished()
    // {
    //     return true;
    // }

}
