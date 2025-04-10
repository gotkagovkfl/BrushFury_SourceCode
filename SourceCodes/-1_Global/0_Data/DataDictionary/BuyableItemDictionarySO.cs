using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using BW;

[CreateAssetMenu(fileName = "BuyableItemDictionary", menuName = "SO/Dictionary/BuyableItem", order = int.MaxValue)]
public class BuyableItemDictionarySO : ScriptableObject
{
    [SerializeField] protected List<BuyableItemSO> list = new(); 
    
    
    public SerializableDictionary<string,BuyableItemSO> dic = new(); 

    protected System.Random random = new System.Random();

    //================================================================================================

    // 유니티 에디터에서 값이 변경될 때마다 호출되는 메서드
    private void OnValidate()
    {
        // 딕셔너리와 리스트를 동기화
        SyncDictionaryWithList();
    }

    // 딕셔너리를 리스트와 동기화하는 메서드
    private void SyncDictionaryWithList()
    {
        // 리스트에서 null인 값이 없을 때, 
        if (list.Any(x=>x==null))
        {
            return;
        }
        
        
        list = list.OrderBy(x=> x.id).ToList();    // id로 오름차순
        
        dic.Clear();

        // 사전에 리스트의 데이터 등록 
        foreach (BuyableItemSO data in list)
        {
            if (data==null)
            {
                return;
            }
            
            string id = data.id;
            if (!dic.ContainsKey(id))
            {
                dic[id] = data;
            }
        }
    }

    //================================================

    /// <summary>
    /// 중복 없이 dataNum개의 데이터를 list에서 가져온다.
    /// </summary>
    /// <param name="targetNum"></param>
    /// <returns></returns>
    public List<BuyableItemSO> GetRandomData( int targetNum, List<BuyableItemSO> exception=null)
    {
        // 
        List<BuyableItemSO> ret = list.GetRandomDataWithException(targetNum,exception);
        //
        return ret;
    }


    /// <summary>
    /// 중복 없이 dataNum개의 데이터를 list에서 가져온다.
    /// </summary>
    /// <param name="targetNum"></param>
    /// <returns></returns>
    // public List<BuyableItemSO> GetRandomDisplableData( int targetNum)
    // {
    //     //
    //     List<BuyableItemSO> exception = list.Where(x=>x.baseData.CanDisplay()==false).ToList();
    //     List<BuyableItemSO> ret = list.GetRandomDataWithException(targetNum, exception);
        
    //     //
    //     return ret;
    // }



    public BuyableItemSO GetData(string id)
    {
        if (dic.ContainsKey(id))
        {
            return dic[id];
        }

        return null;
    }

}
