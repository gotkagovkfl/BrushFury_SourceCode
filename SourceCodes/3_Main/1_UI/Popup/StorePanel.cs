using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class StorePanel : GamePlayPanel
{
    //
    [Header("UI")]
    [SerializeField] TextMeshProUGUI text_title;
    [SerializeField] GoldCountUI goldCountUI;
    [SerializeField] Button btn_reroll;
    [SerializeField] TextMeshProUGUI text_reroll;
    //
    [SerializeField] Button btn_close;

    //

    [SerializeField] StoreGoodsUI goodsUI;
    [SerializeField] PlayerStatusTable playerStatusTable;
    [SerializeField] PlayerItemListUI playerItemList;


    //
    [Header("data")]
    int currRerollCost=0;
    int initialRerollCost;



    //======================================================================
    protected override void Init()
    {
        btn_reroll.onClick.AddListener(TryReroll);
        // btn_close.onClick.AddListener( GamePlayManager.Instance.CloseStore );

        goodsUI.Init();

        GameEventManager.Instance.onChangePlayerGold.AddListener( (int a,int b)=> { UpdateRerollCostText();  
                                                                                    UpdateStatusTable();
                                                                                    UpdateAcquiredItems(); }); // 돈변화가 생기면 능력치 변화도 생긴거니. 
    }

    protected override void OnOpen()
    {
        int stageLevel = GameManager.Instance.userData.currStageNum;
        text_title.SetText($"상점 ({stageLevel+1}단계)");

        ResetGoods(stageLevel);         // 상품 목록 재진열
        UpdateStatusTable();            //스탯 테이블 업데이트
        UpdateAcquiredItems();          //플레이어 아이템 리스트 업뎃
        InitRerollCost(stageLevel);     // 리롤 비용 초기화
        UpdateRerollCostText();         // 리롤 텍스트 초기세팅


        SoundManager.Instance.Invoke(Player.Instance.t, SoundEventType.Store_Open);
    }

    protected override void OnClose()
    {
        SoundManager.Instance.Invoke(Player.Instance.t, SoundEventType.Store_Close);
    }

    //=====================================================================

    /// <summary>
    ///  상점 아이템 재진열 - 상점 열릴때나, 리롤했을 때, 
    /// </summary>
    void ResetGoods(int currStageLevel)
    {
        // List<BuyableItemSO> goodsDatalist = ResourceManager.Instance.buyableItemDic.GetRandomDisplableData(4);
        // goodsUI.ResetGoods(goodsDatalist); // 상점아이테 진열  
    }

    void UpdateStatusTable()
    {
        playerStatusTable.UpdateTable();

    }

    void UpdateAcquiredItems()
    {
        playerItemList.UpdateList();
    }

    /// <summary>
    /// 리롤 초기비용 설정. 
    /// </summary>
    /// <param name="currStageLevel"></param>
    void InitRerollCost(int currStageLevel)
    {
        initialRerollCost = (currStageLevel +1) * 10;
        currRerollCost = initialRerollCost;
    }

    /// <summary>
    ///  리롤 비용을 증가시킴 ( 리롤했을때, ) - 공식은 아직 내맘임. 
    /// </summary>
    void IncreaseRerollCost()
    {
        currRerollCost += (int)(initialRerollCost*0.5f);
    }

    /// <summary>
    /// 리롤트라이. 
    /// </summary>
    void TryReroll()
    {
        int gold = Player.Instance.status.gold;

        if (gold >= currRerollCost)
        {
            //
            Player.Instance.status.UseGold(currRerollCost);     // 이거때문에 리롤텍스트 2번 업데이트되긴함. 

            //
            IncreaseRerollCost();
            UpdateRerollCostText(); 

            //
            int currStageLevel = GameManager.Instance.userData.currStageNum;
            ResetGoods(currStageLevel);

            SoundManager.Instance.Invoke(Player.Instance.t, SoundEventType.Store_Buy);
        }
        else
        {
            Debug.Log("리셋 비용 부족");
            SoundManager.Instance.Invoke(Player.Instance.t, SoundEventType.Store_Lack);
        }
    }

    /// <summary>
    /// 플레이어 돈 변경시 리롤 - 이게 사실 increaseRerllCost 직후에 불려야하는데, 어차피 골드 쓰니까 불리긴함
    /// </summary>
    void UpdateRerollCostText()
    {
        bool canReroll = Player.Instance.status.gold >= currRerollCost;
        Color textColor = canReroll?Color.white:Color.red;
        string text = $"재입고 {currRerollCost}<size=40><sprite=12></size>";

        // 
        text_reroll.SetText(text); 
        text_reroll.color = textColor;
    }

}
