using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpineEffect : GameEffect
{
    public MeshRenderer mr;
    public MeshFilter mf;

    public abstract float lifeTime {get;}


    public override void OnCreatedInPool()
    {
        mr = GetComponent<MeshRenderer>();
        mf = GetComponent<MeshFilter>();
    }

    public override void OnGettingFromPool()
    {
        
    }

    protected override IEnumerator DestroyCondition()
    {
        yield return new WaitForSeconds(lifeTime);
    }
}
