using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


public abstract class EnemySpawnDataSO : GameData
{
    public override string id => $"Spawn_{secondId}";
    
    public int stageLevel;    // 등급
    [SerializeField] string secondId;
    
    
    //==================================================
    public abstract IEnumerator SpawnRoutine(Stage currStage,float startTime );
}
