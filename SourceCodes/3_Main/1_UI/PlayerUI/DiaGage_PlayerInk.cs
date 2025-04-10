using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaGage_PlayerInk : DiaGage
{
    
    protected override void OnValueChanged_Custom()
    {
        if( Player.Instance==null || Player.Instance.initialized ==false)
        {
            return;
        }

        
        text.SetText($"{Player.Instance.status.currInk}/{Player.Instance.status.maxInk.value}");
    }
}
