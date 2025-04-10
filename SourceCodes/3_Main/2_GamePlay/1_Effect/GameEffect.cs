using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEffect : BwPoolObject
{
    public override PoolType poolType => PoolType.Effect; 


    // public Vector3 initPos;


    public void Init()
    {
        // this.initPos = initPos;
        
        Init_Custom();
        StartCoroutine(DestroyRoutine());
    }

    public void SetDirection(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        myTransform.rotation = Quaternion.Euler(0, angle, 0);
    }


    protected abstract void Init_Custom();


    protected IEnumerator DestroyRoutine()
    {
        yield return DestroyCondition();
        // 풀링. 

        Return();
    }

    protected abstract IEnumerator DestroyCondition();



    public void Return()
    {
        PoolManager.Instance.effectPoolSys.Return(this);
    }

}
