using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[CreateAssetMenu(fileName = "EffectPoolDataList", menuName = "SO/PoolDataList/Effect", order = int.MaxValue)] 
public class EffectPoolListSO : ScriptableObject
{   
    [SerializeField] List<GameEffect> list;
    
    // public SerializableDictionary<string, GameObject> defaultEffectObjects; 
    // public SerializableDictionary<EffectEvent, EffectPoolData> dic;



    void OnValidate()
    {
        SyncList();
    }


    void SyncList()
    {
        // defaultEffectObjects?.Clear();
        // foreach(GameObject prefab in list)
        // {
        //     GameEffect effect = prefab.GetComponent<GameEffect>();
            
        //     if(defaultEffectObjects.ContainsKey(effect.id)==false)
        //     {
        //         defaultEffectObjects[effect.id] = prefab;
        //     }
        // }
    }
}

