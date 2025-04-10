using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyMoveType
{
    None,
    SimpleApproach,
    EncirclingApproach,
    Kite,
    
}


public abstract class EnemyDestinationSetter  
{
    protected Enemy enemy;
    
    protected Vector3 currDest;
    protected float destDistSqr;


    public EnemyDestinationSetter(Enemy enemy)
    {
        this.enemy = enemy;

        //
        currDest =  enemy.myTransform.position;
    }

    public void TryUpdate(float currAbilityRange)
    {
        destDistSqr = (currDest - enemy.myTransform.position).sqrMagnitude;
        
        Update_Custom(currAbilityRange);
        enemy.status.targetPosition = currDest;
    }

    protected abstract void Update_Custom(float currAbilityRange);
}
