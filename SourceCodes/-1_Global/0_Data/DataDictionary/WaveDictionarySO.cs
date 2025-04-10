using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(fileName = "WaveDictionary", menuName = "SO/Dictionary/Wave", order = int.MaxValue)]
public class WaveDictionarySO : ScriptableObject
{
    // [SerializeField] StageGenerationConfigSO config; 
    
    //
    [Header("Survival Time")]
    [SerializeField] SerializableDictionary<int, float> totalTargetSurvivalTime = new();     // key: stageLevel

    [Space(10)]
    [Header("Enemy Spawn Info")]
    [SerializeField] protected List<EnemySpawnDataSO> list_baseSpawnData = new();
    [SerializeField] protected List<EnemySpawnDataSO> list_specialSpawnData = new();    // 보스나 특수한 애들 넣을거임. 
    
    [Space(10)]
    [SerializeField] SerializableDictionary<int, List<EnemySpawnDataSO>> totalEnemySpawnData = new();        // key: stageLevel,

    //==============================================================================
    // public StageMissionInfo GetStageMissionInfo(StageNode nodeData)
    // {
    //     int stageLevel = nodeData.level;
    //     // stageLevel에 대한 예외 처리 필요없음. config에서 생성부터, 제한하고 계속 동기화하기 때문. 
    //     float targetSurvivalTime = totalTargetSurvivalTime[stageLevel];
    //     List<EnemySpawnDataSO> enemySpawnData = totalEnemySpawnData[stageLevel];

    //     StageMissionInfo ret = new(targetSurvivalTime, enemySpawnData);
    //     return ret;

    // }

    //===========================================================================================

    // 유니티 에디터에서 값이 변경될 때마다 호출되는 메서드
    private void OnValidate()
    {



    }


    void SyncTargetSurvivalTime(int maxStageLevel)
    {
        // data 초기화. 
        float initvalue = 60;
        for(int i=0;i< maxStageLevel;i++)
        {
            if(totalTargetSurvivalTime.ContainsKey(i)==false)
            {
                totalTargetSurvivalTime[i] = initvalue;
            }
            else
            {
                initvalue = totalTargetSurvivalTime[i];
            }
        };
    }


    // 딕셔너리를 리스트와 동기화하는 메서드
    private void SyncEnemySpawnData(int maxStageLevel)
    {
        // 리스트에서 null인 값이 없을 때에만 진행하도록. 
        if (list_baseSpawnData.Any(x=>x==null) || list_specialSpawnData.Any(x=>x==null))
        {
            return;
        }
        
        // data 초기화. 
        totalEnemySpawnData.Clear();
        for(int i=0;i< maxStageLevel;i++)
        {
            totalEnemySpawnData[i] = new();
        }

        //
        list_baseSpawnData = list_baseSpawnData.Where(x=>x.stageLevel <= maxStageLevel ). ToList(); // maxRank를 넘지 않는 웨이브들만 살리기.  
        list_specialSpawnData = list_specialSpawnData.Where(x=>x.stageLevel <= maxStageLevel ). ToList(); // maxRank를 넘지 않는 웨이브들만 살리기.  

        List<EnemySpawnDataSO> concatList = list_baseSpawnData.Concat(list_specialSpawnData).ToList(); // 리스트 합치기. 분리한 이유는 단순히 보기 편하라고. 

    
        // 사전에 리스트의 데이터 등록  
        foreach (EnemySpawnDataSO spawnData in concatList)
        {
            if (spawnData ==null)
            {
                continue;
            }
            
            int stageLevel = spawnData.stageLevel;
            totalEnemySpawnData[stageLevel].Add(spawnData);
        }
    }



    //================================================

    

}
