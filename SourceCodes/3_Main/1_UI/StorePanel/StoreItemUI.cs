using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemUI : MonoBehaviour
{
    public ItemDataSO optionData;
    
    [SerializeField] Image img_icon;                    // 아이템 아이콘 
    [SerializeField] TextMeshProUGUI text_itemName;     // 아이템 이름
    [SerializeField] TextMeshProUGUI text_itemType;     // 아이템 타입 - 능력치, 기본공격, 
    [SerializeField] TextMeshProUGUI text_itemDesc;     // 아이템 상세 설명
    [SerializeField] Button btn_buy;                    // 버튼 - 구매 불개능시 비활성화
    // [SerializeField] TextMeshProUGUI text_itemCost;     // 가격 - 구매불개능시 빨간글씨, 구매시 구매완료 
    [SerializeField] GameObject vayle;                  // 구매완료시 베일 생김t

    
    
    //=============================================================================
    
    public void Init(Action btnClickAction)
    {
        btn_buy.onClick.AddListener(TryBuy);
        btn_buy.onClick.AddListener( ()=>btnClickAction());
    }
    
    /// <summary>
    /// 상점이 초기화될 때마다 호출됨. 
    /// </summary>
    /// <param name="data"></param>
    public void OnReset(ItemDataSO data)
    {
        gameObject.SetActive(true);
        vayle.SetActive(false);
        btn_buy.interactable= true;

        //
        this.optionData = data;

        img_icon.sprite = data.sprite;
        text_itemName.SetText( data.dataName );
        text_itemType.SetText( data.typeKor );      // 타입을 스트링으로 변환하는 과정 필요
        text_itemDesc.SetText( data.description );

        // SetBuyButton();
        
        
    }
    
    public void SetUnavialable()
    {
        gameObject.SetActive(false);
    }



    /// <summary>
    ///  구매버튼 눌렀을 때, 
    /// </summary>
    void TryBuy()
    {
        //
        optionData.TryGet();
        vayle.SetActive(true);
        btn_buy.interactable= false;

        // 구매완료 ui 세팅
        SoundManager.Instance.Invoke(Player.Instance.t, SoundEventType.Store_Buy);
    
        
    }

}
