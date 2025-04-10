using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ResourceManager : Singleton<ResourceManager>
{
    // public readonly string enemyDataPath = "EnemyData";
    // public readonly string defaultEnemyId = "000";


    // public readonly string dropItemDataPath = "DropItemData";
    // public readonly string defaultDropItemId = "00";


    // public SerializedDictionary<string, EnemyDataSO> enemyData = new();

    // public SerializedDictionary<string, DropItemDataSO> dropItemData = new();


    
    // public readonly string itemDataPath = "00_Data/itemDictionary";
    // public ItemDictionarySO itemDic;


    public readonly string areaIndicatorDataPath = "00_Data/areaIndicatorDictionary";
    public AreaIndicatorDictionarySO areaIndicatorDic;


    // public StageRewardDictionarySO stageRewardDic;
    // public StagePrefabDictionarySO stagePrefabDic;

    public StatusEffectDictionarySO statusEffectDic;
    

    #region Stage
    public GameObject prefab_stagePortal;

    #endregion 

    #region Config
    

    // public WaveDictionarySO waveDic;

    
    #endregion

    //=================================================


    public void Init()
    {
        // foreach(EnemyDataSO enemy in Resources.LoadAll<EnemyDataSO>(enemyDataPath))
        // {
        //     enemyData[enemy.id] = enemy;
        // }

        // foreach(DropItemDataSO itemData in Resources.LoadAll<DropItemDataSO>(dropItemDataPath))
        // {
        //     dropItemData[itemData.id] = itemData;
        // }

        // itemDic = Resources.Load<ItemDictionarySO>(itemDataPath);
        areaIndicatorDic  = Resources.Load<AreaIndicatorDictionarySO>(areaIndicatorDataPath);
    }

    //

    // public EnemyDataSO GetEnemyData(string id)
    // {
    //     if (enemyData.ContainsKey(id))
    //     {
    //         return enemyData[id];
    //     }

    //     return enemyData[defaultEnemyId];
    // }


    
    // public DropItemDataSO GetDropItemData(string id)
    // {
    //     if (dropItemData.ContainsKey(id))
    //     {
    //         return dropItemData[id];
    //     }

    //     return dropItemData[defaultDropItemId];
    // }
}
