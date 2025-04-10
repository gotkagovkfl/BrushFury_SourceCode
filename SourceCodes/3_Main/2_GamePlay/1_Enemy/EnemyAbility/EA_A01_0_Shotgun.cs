using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// 산탄총처럼 다수의 불꽃을 한번에 발사한다. 
/// </summary>
[CreateAssetMenu(fileName = "EA_A01_0", menuName = "SO/EnemyAbility/A01_0_샷건")]
public class EA_A01_0_Shotgun : EnemyAbilitySO
{
    [Header("Extra")]
    [SerializeField] EnemyProjectile prefab_fireBall;
    public int bulletCount = 4;
    public float anglePerBullet = 15f;

    public float movementSpeed = 5f;
    public float lifeTime = 5f;
    


    public override bool UsageConditions(Enemy enemy)
    {
        return true;
    }

    public override Transform GetTarget(Enemy enemy)
    {
        return enemy.t_target;
    }


    public override AreaIndicator ShowCastingIndicator(Enemy enemy, Vector3 targetPos)
    {
        return null;
    }

    public override  UniTask ActivationTask(Enemy enemy, Vector3 targetPos, CancellationToken token)
    {
        Vector3 initPos = enemy.myTransform.position.WithStandardHeight();
        targetPos = targetPos.WithStandardHeight();
        
        float currAngle = - (bulletCount-1) * anglePerBullet * 0.5f; 
        for(int i=0;i<bulletCount; i++)
        {
            var ep = PoolManager.Instance.enemyProjectilePoolSys.GetEP(enemy,this, prefab_fireBall, initPos, targetPos);
            if( ep is EP_A01_0_ShotGunBullet shotgunBullet)
            {
                shotgunBullet.SetDirAndMove(currAngle);
            }
            currAngle+= anglePerBullet;
        }


        
        return UniTask.CompletedTask; 
    }
}
