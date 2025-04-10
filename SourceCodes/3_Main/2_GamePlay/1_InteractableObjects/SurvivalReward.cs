using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalReward : InteractiveObject
{
    protected override string inspectingText => "시장";

    // protected override float TargetInteractingTime => 0;


    //========================================================================
    protected override void OnInspect_Custom(bool isOn)
    {
        // text.gameObject.SetActive(isOn);
    }

    protected override void OnInteract_Custom()
    {
        // GamePlayManager.Instance.OpenStore();
        Destroy(gameObject);
    }

    //========================================================================
    public void Init()
    {
        
    }
}
