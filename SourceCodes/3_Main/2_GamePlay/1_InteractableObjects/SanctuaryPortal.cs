using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class SanctuaryPortal :InteractiveObject
{
    protected override string inspectingText => "전투시작";


    protected override void OnInspect_Custom(bool isOn)
    {

    }

    protected override void OnInteract_Custom()
    {
        //
        GoToMainScene();
    }

    
    //
    void GoToMainScene()
    {
        UnderWorldManager.Instance.LeaveUnderWorld();
    }
}
