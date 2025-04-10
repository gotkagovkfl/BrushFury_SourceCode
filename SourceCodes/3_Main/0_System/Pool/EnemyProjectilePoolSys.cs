using System.Collections;
using System.Collections.Generic;
using BW;
using UnityEngine;

public class EnemyProjectilePoolSys : BwPoolSystem
{
    
    
    
    public EnemyProjectile GetEP(Enemy enemy, EnemyAbilitySO eaData, EnemyProjectile ep, Vector3 initPos, Vector3 targetPos)
    {
        EnemyProjectile po = Get<EnemyProjectile>( ep, initPos);
        po.Init( enemy,eaData,initPos, targetPos);

        return po;
    }
}
