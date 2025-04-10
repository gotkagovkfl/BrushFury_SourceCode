using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using System;


[CreateAssetMenu(fileName = "EnemyDictionary", menuName = "SO/Dictionary/Enemy", order = int.MaxValue)]
public class EnemyDictionarySO : ScriptableObject
{
    [SerializeField] protected List<GameObject> list = new(); 
    
    
    public SerializableDictionary<string,EnemyDataSO> totalData = new(); 

    // public SerializableDictionary<string,EnemyPoolData> totalPoolData = new();

    protected System.Random random = new System.Random();


    //=======================================================================================
    
    public EnemyDataSO GetData(string id)
    {
        if (totalData.TryGetValue(id, out EnemyDataSO data))
        {
            return data;
        }
        
        return totalData["001"];
    }





    // 유니티 에디터에서 값이 변경될 때마다 호출되는 메서드
    // private void OnValidate()
    // {
        // 딕셔너리와 리스트를 동기화
        // Sync();
    // }

    // 딕셔너리를 리스트와 동기화하는 메서드
    // private void Sync()
    // {
    //     // 리스트에서 null인 값이 없을 때, 
    //     if (list.Any(x=>x==null || x.GetComponent<Enemy>().data==null) )
    //     {
    //         return;
    //     }


    //     list = list.OrderBy(x=> x.GetComponent<Enemy>().data.id).ToList();    // id로 오름차순
        
    //     totalData.Clear();
    //     totalPoolData.Clear();

    //     // 사전에 리스트의 데이터 등록 
    //     foreach (GameObject enemyObject in list)
    //     {
    //         Enemy enemy = enemyObject.GetComponent<Enemy>();
    //         string id = enemy.data.id;
    //         // 데이터 종류. 
    //         if (!totalData.ContainsKey(id))
    //         {
    //             totalData[id] = enemy.data;
    //         }

    //         // 풀데이터 
    //         if(!totalPoolData.ContainsKey(id))
    //         {
    //             EnemyPoolData epd = new();
    //             epd._name = $"{id}";
    //             epd._component = enemy;
                
                
    //             totalPoolData[id] = epd;
    //         }
    //     }
    // }


}
