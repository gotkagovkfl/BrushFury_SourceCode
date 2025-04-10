using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_001_0_SatelliteCenter : PlayerProjectile
{
    protected override string id => "001_0";

    public float rotateSpeed; 


    //====================================================
    protected override void Init_Custom()
    {

    }

    //===============================================
    void FixedUpdate()
    {
        Vector3 euler = Vector3.up * rotateSpeed * Time.fixedDeltaTime;
        myTransform.Rotate( euler );
    }


    
    public void SetRotateSpeed( float rotateSpeed )
    {
        this.rotateSpeed = rotateSpeed; 
    }


}


