using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreGoodsUI : MonoBehaviour
{
    [SerializeField] Transform t_items;
    [SerializeField] List<StoreItemUI> storeItems;

    public void Init()
    {
        storeItems=new();
        for(int i=0;i<t_items.childCount;i++)
        {
            storeItems.Add(t_items.GetChild(i).GetComponent<StoreItemUI>());
        }
    }

    /// <summary>
    /// 상점 초기화할 때마다 호출됨. 
    /// </summary>
    /// <param name="goodsDataList"></param>
    public void ResetGoods(List<BuyableItemSO> goodsDataList)
    {

        for(int i=0; i<storeItems.Count; i++)
        {
            StoreItemUI storeItem = storeItems[i];
            
            // 정상인경우,
            if(i< goodsDataList.Count)
            {
                BuyableItemSO goodsData = goodsDataList[i];

                // storeItem.OnReset( goodsData );
            }
            // 인풋 데이터와 목록의 수가 맞지 않는 경우. 
            else
            {
                storeItem.SetUnavialable();
            }   
        }
    }
}
