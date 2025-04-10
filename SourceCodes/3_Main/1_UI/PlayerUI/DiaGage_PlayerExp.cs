using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaGage_PlayerExp : DiaGage
{   
    

    protected override void OnValueChanged_Custom()
    {
        if( Player.Instance==null || Player.Instance.initialized ==false)
        {
            return;
        }


        text.SetText($"{Player.Instance.status.level.value}단\n{m_value/m_maxValue * 100:00.0}%");


        if(Player.Instance.status.canLevelUp)
        {
            img_fill.color = Color.magenta;    
        }
        else
        {
            img_fill.color = fillColor;    
        }
    }
}

