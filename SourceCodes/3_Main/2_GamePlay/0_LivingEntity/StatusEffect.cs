using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect 
{
    public float enterTime;
    public float duration;

    public StatusEffect()
    {

    }



    public void OnEnter()
    {
        OnEnterEffect();
    }

    public void OnUpdate()
    {
        OnUpdateEffect();
    }

    public void OnExit()
    {
        OnExitEffect();
    }


    protected abstract void OnEnterEffect();
    protected abstract void OnUpdateEffect();
    protected abstract void OnExitEffect();
}
