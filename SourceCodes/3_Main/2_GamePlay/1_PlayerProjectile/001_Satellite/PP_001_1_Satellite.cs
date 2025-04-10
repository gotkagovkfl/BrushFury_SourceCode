using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_001_1_Satellite : PlayerProjectile
{
    protected override string id => "001_1";
    

    float targetDist;

    //====================================================
    protected override void Init_Custom()
    {
        
    }

    public void InitTargetDist( float targetDist)
    {
        this. targetDist = targetDist;
    }


    public void UpdateRatio(float ratio)
    {
        Vector3 temp = followOffset * targetDist * ratio;
        Vector3 targetPosition = new Vector3(temp.x, 1, temp.z);
        
        myTransform.localPosition = targetPosition;
    }
}

