using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "StageMission", menuName = "SO/StageMission")]
public class StageMissionSO : ScriptableObject
{
    public int stageNum;

    [Header("Mission Setting")]
    public BossSpawnData bossSpawnData;

    public List<EnemySpawnDataSO> enemySpawnData= new();


}







[Serializable]
public class BossSpawnData
{
    public float bossSpawnTime;
    public Enemy bossPrefab;
}