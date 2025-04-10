using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPanel : GamePlayPanel
{
    [SerializeField] StoreItemUI prefab_optionUI;

    [SerializeField] Transform t_options;
    [SerializeField] List<StoreItemUI> selectableOptions;
    
    // [SerializeField ] Button TestCloseBtn;
    
    protected override void Init()
    {
        // TestCloseBtn.onClick.AddListener(Close);

        //
        selectableOptions = new();
        for(int i=0;i<t_options.childCount;i++)
        {
            StoreItemUI newOptionUI = t_options.GetChild(i).GetComponent<StoreItemUI>();
            newOptionUI.Init( OnOptionSelected );
            selectableOptions.Add(newOptionUI);
        }
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen()
    {
        int selectableOptionCount = (int)Player.Instance.status.selectableOptionCount.value;
        int currOptionsCount = selectableOptions.Count;
        
        int diff = selectableOptionCount - currOptionsCount;
        // 만들어야하는 경우.
        if (diff > 0 )
        {
            for(int i=0;i<diff;i++)
            {
                StoreItemUI newOptionUI = Instantiate( prefab_optionUI.gameObject, t_options).GetComponent<StoreItemUI>();
                newOptionUI.Init(OnOptionSelected);
                selectableOptions.Add(newOptionUI);
            }
        }
        // 부숴야하는 경우.
        else if (diff < 0)
        {
            for(int i= currOptionsCount -1 ;i >=currOptionsCount + diff;i--)
            {
                StoreItemUI targetOptionUI = selectableOptions[i];
                selectableOptions.RemoveAt(i);
                Destroy( targetOptionUI.gameObject );
            }
        }
    }

    //===================================================================================

    void OnOptionSelected()
    {
        Close();
    }



    /// <summary>
    /// 상점 초기화할 때마다 호출됨. 
    /// </summary>
    /// <param name="goodsDataList"></param>
    public void FillOptionData(List<ItemDataSO> optionDataList, ItemDataSO defaultItemData )
    {

        for(int i=0; i<selectableOptions.Count; i++)
        {
            StoreItemUI storeItem = selectableOptions[i];
            
            // 정상인경우,
            if(i< optionDataList.Count)
            {
                ItemDataSO  optionData = optionDataList[i];
                storeItem.OnReset( optionData );
            }
            // 인풋 데이터와 목록의 수가 맞지 않는 경우 기본 아이템으로 채우기.  - 모든 강화를 마쳤을 때 나타남. 
            else
            {
                storeItem.OnReset( defaultItemData);
            }   
        }
    }

}
