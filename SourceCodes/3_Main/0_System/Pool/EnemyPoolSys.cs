using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BW;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class EnemyPoolSys : BwPoolSystem
{

    // [SerializeField] EnemyDictionarySO enemyData;

    // [SerializeField] EnemySpawner enemySpawner;

    //================================================================================
    public Enemy GetEnemy(string poolId, Vector3 initPos)
    {
        Enemy po = Get<Enemy>( poolId, initPos );
        po.Init();

        return po;
    }


    public Enemy GetEnemy(Enemy enemyPrefab, Vector3 initPos)
    {
        Enemy po = Get<Enemy>( enemyPrefab, initPos );
        po.Init();

        return po;
    }
    //=====================================================================
}
