using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BW;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// using Redcode.Pools;

[Serializable]
internal struct PoolData
{
    [SerializeField]
    public string _name;

    public string Name => _name;

    [SerializeField]
    public Component _component;

    public Component Component => _component;

    [SerializeField]
    [Min(0)]
    private int _count;

    public int Count => _count;

    [SerializeField]
    private Transform _container;

    public Transform Container => _container;

    [SerializeField]
    private bool _nonLazy;

    public bool NonLazy => _nonLazy;
}

/// <summary>
/// Pool manager. You can set options for it in editor and then use in game. <br/>
/// It creates specified pools in Awake method, which then you can find with <b>GetPool</b> methods and call its methods.
/// </summary>
public class PoolManager : Singleton<PoolManager>
{
    /*
    풀링 오브젝트 목록

     1. 사운드      - 근데 사운드는 별도로 진행 ( 사운드는 다른 씬에서도 쓰이기 때문 )
     2. 이펙트
     3. 드랍아이템
     4. 적
     5. 적 투사체
     6. 플레이어 투사체

    */


    // public SerializableDictionary<PoolType, BwPoolSystem > poolSystems;

    //
    public EffectPoolSys effectPoolSys;
    public DropItemPoolSys dropItemPoolSys;
    public EnemyPoolSys enemyPoolSys;
    public EnemyProjectilePoolSys enemyProjectilePoolSys;
    public PlayerProjectilePoolSys playerProjectilePoolSys;

    public AreaIndicatorPoolManager areaIndicatorPoolManager;
    //


    public bool initialized {get;private set;}


    //==================================================================================

    public void Init()
    {
        //
        effectPoolSys?.Init();
        dropItemPoolSys?.Init();
        enemyPoolSys?.Init();
        enemyProjectilePoolSys?.Init();
        playerProjectilePoolSys?.Init();
        areaIndicatorPoolManager?.Init();
        //

        initialized = true;
    }



    //===========================

    //==============================================================================  

    //=====================================================================
    

    public void GetItem(int num )
    {
        Vector3 initPos = Stage.Instance.GetRandomNearbySpawnPoint();

        switch(num)
        {
            case 0:
                dropItemPoolSys.GetExp(0,initPos);
                break;

            case 1:
                dropItemPoolSys.GetHpUp(1,initPos);
                break;

            case 2:
                dropItemPoolSys.GetMagnet(2,initPos);
                break;

            case 3:
                dropItemPoolSys.GetAmulet(3,initPos);
                break;


            case 4:
                dropItemPoolSys.GetLuckyPouch(4,initPos);
                break;

        }

        

    }

}

