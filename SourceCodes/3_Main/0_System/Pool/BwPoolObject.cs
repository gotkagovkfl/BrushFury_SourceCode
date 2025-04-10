using System.Collections;
using System.Collections.Generic;
using UnityEngine;






public abstract class BwPoolObject : MonoBehaviour, IPoolObject
{
    public Transform myTransform;
    public abstract PoolType poolType {get;}
    protected abstract string id {get;}
    public string poolId => $"{poolType}_{id}";
    public Vector3 initPos;

    public abstract void OnCreatedInPool();
    
    public abstract void OnGettingFromPool();
}







