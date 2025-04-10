using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : Enemy
{
    [SerializeField] EnemyUI enemyUI;

    protected override void Init_Custom()
    {
        // 체력바 생성
        enemyUI?.Init(this);
    }

    protected override void OnDie_Custom()
    {
        // 체력바 삭제
        // 스테이지 종료
        enemyUI?.OnEnemyDeath(this);
    }

    protected override void OnHit()
    {
        // 체력바 업데이트
        enemyUI?.OnHit(this);
    }
}
