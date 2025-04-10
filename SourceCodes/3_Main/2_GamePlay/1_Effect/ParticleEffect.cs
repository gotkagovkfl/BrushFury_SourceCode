using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParticleEffect : GameEffect
{
    protected ParticleSystem ps;


    public override void OnCreatedInPool()
    {
        ps = GetComponent<ParticleSystem>();
        
    }

    public override void OnGettingFromPool()
    {
        
    }


    //===============================================
    protected override void Init_Custom()
    {

    }



    //
    protected override IEnumerator DestroyCondition()
    {
        yield return new WaitUntil( ()=> ps.IsAlive() == false);
    }
}
