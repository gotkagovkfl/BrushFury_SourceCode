using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StagePrefabDictionary", menuName = "SO/Dictionary/StagePrefab", order = int.MaxValue)]
public class StagePrefabDictionarySO : ScriptableObject
{
    [Header("Stage")]
    // [SerializeField] GameObject defaultStagePrefab;
    [SerializeField] SerializableDictionary<int, GameObject> stagePrefabs;


    [Header("Portal")]
    [SerializeField] GameObject defaultPortalPrefab; 
    // [SerializeField] SerializableDictionary<StageNodeType, GameObject> stagePortalPrefabs;


    //=========================================================================================

    public GameObject GetStagePrefab(int currStageNum)
    {
        if( stagePrefabs.TryGetValue(currStageNum, out GameObject ret))
        {
            return ret;
        }
        throw new("안돼!!");
    }
    
    // public GameObject GetPortalPrefab(StageNodeType type)
    // {
        
    // }


}
