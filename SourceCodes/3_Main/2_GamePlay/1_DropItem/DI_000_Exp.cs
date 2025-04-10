using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DI_000_Exp : DropItem
{

    public void StackExp(float amount)
    {
        value+=amount;
        // myTransform.localScale *= 1.1f;     // 이미지 바꾸는걸로 변경. 
    }

    protected override void Init_Custom()
    {
        
    }

    protected override void Return_Custom()
    {
        PoolManager.Instance.dropItemPoolSys.RemoveExp(this);
    }
}
