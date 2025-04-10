using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityStatusEffect 
{
    public EntityStatus targetStatus;
    // public PlayerStatusEffectSO data; 

    public float enterTime;
    public float totalDuration;


    public bool cached;
    public bool activated;

    //=====================================================================================
    public void Init(EntityStatus targetStatus)
    {
        this.targetStatus = targetStatus;
        Init_Custom();
    }


    public void OnEnter(float targetDuration)
    {
        //
        if (activated==true) 
        {
            OnExit();
        }
        
        activated = true;
        
        enterTime = Time.time;
        totalDuration = targetDuration;

        OnEnter_Custom();
        // Debug.Log($"enter-- {activated}");
    }

    public void OnUpdate()
    {
        //
        if(activated == false)
        {
            return;
        }

        //
        float currDuration =  Time.time - enterTime;
        OnUpdate_Custom(currDuration);

        // 지속시간 검사. 
        if (currDuration >=  totalDuration)
        {
            OnExit();
        }
    }

    public void OnExit()
    {
        activated = false;
        OnExit_Custom();
        // Debug.Log($"exit-- {activated}");
    }



    //
    protected abstract void Init_Custom();
    protected abstract void OnEnter_Custom();
    protected abstract void OnExit_Custom();
    protected abstract void OnUpdate_Custom(float currDuration);
}
