using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EDS_SimpleApproach : EnemyDestinationSetter  
{
    // [Header(" Dest Offset")
    
    //==============================================================
    public EDS_SimpleApproach(Enemy enemy) : base(enemy)
    {

    }

    protected override void Update_Custom(float currAbilityRange)
    {

        currDest = enemy.t_target.position ;
    }
}






