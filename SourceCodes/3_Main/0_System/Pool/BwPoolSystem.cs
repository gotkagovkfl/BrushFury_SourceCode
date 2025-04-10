using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum PoolType
{
    None,           // 그냥 기본값
    Sound,          // 소리
    Effect,
    DropItem,
    Enemy,
    EnemyProjectile,
    PlayerProjectile,
    AreaIndicator,
}










public class BwPoolSystem : MonoBehaviour
{   
    
    public PoolType poolType;
    Transform t_container;

    public bool initialized { get;private set;}


    [SerializeField] protected List<BwPoolObject> list_defaultPrefabs;
    [SerializeField] protected SerializableDictionary<string, Pool<BwPoolObject> > _pools;

    

    public void Init()
    {
        t_container = transform;
        //
        InitPools();

        initialized = true;
    }

    void InitPools()
    {
        _pools = new();


        foreach(var prefab in list_defaultPrefabs)
        {
            AddPoolItem(prefab);
        }


    }

    public void AddPoolItem(BwPoolObject poolObject)
    {

        StartCoroutine(AddRoutine( poolObject ));
    }


    public void RemovePoolItem(BwPoolObject  poolObject )
    {
        StartCoroutine(RemoveRoutine( poolObject ));
    }


    IEnumerator AddRoutine(BwPoolObject poolObject)
    {
        yield return new WaitUntil(()=>initialized);
        
        string id = poolObject.poolId;
        // 풀데이터 
        if(_pools.ContainsKey(id) == false)
        {
            var pool = Pool<BwPoolObject>.Create( poolObject, 0, t_container);
            _pools[id] = pool;
        } 
    }

    IEnumerator RemoveRoutine(BwPoolObject poolObject)
    {
        yield return new WaitUntil(()=>initialized);
        
        string id = poolObject.poolId;
        // 풀데이터 
        if(_pools.ContainsKey(id) == true )
        {
            _pools.Remove(id);
        }
    }

    //====================================================================
    Pool<BwPoolObject> GetPool(string poolId)
    {
        if (_pools.TryGetValue(poolId, out var pool))
        {
            return pool;
        }

        throw new InvalidOperationException($"No pool exists for type {poolId}");
    }

    Pool<BwPoolObject> GetPool(BwPoolObject po)
    {
        string poolId = po.poolId;
        if (_pools.TryGetValue(poolId, out var pool))
        {
            return pool;
        }
        else
        {
            var newPool = Pool<BwPoolObject>.Create( po, 0, t_container);
            _pools[poolId] = newPool;
               
            return newPool;
        }
    }

    
    //================================================================
    public T Get<T>(BwPoolObject bwPoolObject,Vector3 initPos) where T:BwPoolObject
    {
        var pool = GetPool(bwPoolObject);
        T poolObject = pool.Get(initPos) as T;       

        return poolObject;
    }
    
    
    
    
    public T Get<T>(string id,Vector3 initPos)   where T:BwPoolObject
    {
        var pool = GetPool(id);
        T poolObject = pool.Get(initPos) as T;       

        return poolObject;
    }



    public void Return(BwPoolObject poolObject)
    {
        var pool = GetPool(poolObject.poolId);
        pool.Take(poolObject);
    }   

}
