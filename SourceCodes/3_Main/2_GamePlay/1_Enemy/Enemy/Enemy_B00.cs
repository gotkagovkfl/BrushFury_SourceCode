using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_B00 : Enemy
{
    protected override void Init_Custom()
    {
        // GamePlayManager.Instance.UpdateEnemyHpSlider(this);
    }

    protected override void OnDie_Custom()
    {
        Stage.Instance.FinishStage();       // 연출도 재생해야지~
    }

    protected override void OnHit()
    {
        // GamePlayManager.Instance.DeactivateEnemyHpSlider();
    }
}
