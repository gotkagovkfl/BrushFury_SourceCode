using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpriteEffect : GameEffect
{
    public SpriteRenderer sr;

    public abstract float lifeTime {get;}

    //===============================================

    public override void OnCreatedInPool()
    {
        sr = GetComponent<SpriteRenderer>();
        
    }
    public override void OnGettingFromPool()
    {
        sr.color = Color.white;
    }

    //==================================================

    protected override void Init_Custom()
    {

    }


    //-------------------------------------------
    protected override IEnumerator DestroyCondition()
    {
        yield return new WaitForSeconds(lifeTime);
    }
    
}
